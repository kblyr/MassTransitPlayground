namespace UserOrchestration.Consumers;

sealed class CreateUserConsumer : IConsumer<CreateUser>
{
    readonly ILogger<CreateUserConsumer> _logger;
    readonly IDbContextFactory<UserDbContext> _contextFactory;
    readonly IMapper _mapper;
    readonly IUserPasswordHashingService _passwordHashingService;

    public CreateUserConsumer(ILogger<CreateUserConsumer> logger, IDbContextFactory<UserDbContext> contextFactory, IMapper mapper, IUserPasswordHashingService passwordHashingService)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _mapper = mapper;
        _passwordHashingService = passwordHashingService;
    }

    public async Task Consume(ConsumeContext<CreateUser> context)
    {
        _logger.LogTrace("Creating {dbContext}", nameof(UserDbContext));
        using var dbContext = await _contextFactory.CreateDbContextAsync(context.CancellationToken).ConfigureAwait(false);
        using var transaction = await dbContext.Database.BeginTransactionAsync(context.CancellationToken).ConfigureAwait(false);

        _logger.LogTrace("Check if username already exists");
        if (await UsernameExistsAsync(dbContext, context.Message.Username, context.CancellationToken))
        {
            _logger.LogTrace("Username already exists");

            _logger.LogTrace("Responding if request");
            if (context.RequestId.HasValue)
                await context.RespondAsync(new UsernameAlreadyExists { Username = context.Message.Username });

            return;
        }

        _logger.LogTrace("Check if email address already exists");
        if (await EmailAddressExistsAsync(dbContext, context.Message.EmailAddress, context.CancellationToken))
        {
            _logger.LogTrace("Email address already exists");

            _logger.LogTrace("Responding if request");
            if (context.RequestId.HasValue)
                await context.RespondAsync(new UserEmailAddressAlreadyExists { EmailAddress = context.Message.EmailAddress });
            
            return;
        }

        _logger.LogTrace("Mapping {mapFrom} to {mapTo}", nameof(CreateUser), nameof(User));
        var user = _mapper.Map<User>(context.Message);
        user.HashedPassword = _passwordHashingService.ComputeHash(context.Message.Password);

        _logger.LogTrace("Adding user locally");
        dbContext.Users.Add(user);

        _logger.LogTrace("Saving changes to database");
        await dbContext.SaveChangesAsync(context.CancellationToken);

        _logger.LogTrace("Mapping {mapFrom} to {mapTo}", nameof(User), nameof(UserCreated));
        var @event = _mapper.Map<UserCreated>(user);
        
        _logger.LogTrace("Responding if request");
        if (context.RequestId.HasValue)
            await context.RespondAsync(@event);

        _logger.LogTrace("Publishing event: {event}", nameof(UserCreated));
        await context.Publish(@event);
        
        _logger.LogTrace("Committing transaction");
        await transaction.CommitAsync(context.CancellationToken).ConfigureAwait(false);
    }

    static async Task<bool> UsernameExistsAsync(UserDbContext context, string username, CancellationToken cancellationToken) => await context.Users
        .AsNoTracking()
        .Where(user => user.Username == username)
        .AnyAsync(cancellationToken);

    static async Task<bool> EmailAddressExistsAsync(UserDbContext context, string emailAddress, CancellationToken cancellationToken) => await context.Users
        .AsNoTracking()
        .Where(user => user.EmailAddress == emailAddress)
        .AnyAsync(cancellationToken);
}
