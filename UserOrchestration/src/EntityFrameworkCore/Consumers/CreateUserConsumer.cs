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
        _logger.DbContext().Creating<UserDbContext>();
        using var dbContext = await _contextFactory.CreateDbContextAsync(context.CancellationToken).ConfigureAwait(false);
        using var transaction = await dbContext.Database.BeginTransactionAsync(context.CancellationToken).ConfigureAwait(false);

        _logger.LogTrace("Check if username already exists");
        if (await UsernameExistsAsync(dbContext, context.Message.Username, context.CancellationToken).ConfigureAwait(false))
        {
            _logger.LogTrace("Username already exists");

            if (context.RequestId.HasValue)
                await context.RespondAsync(new CreateUserFailed { UsernameAlreadyExists = new(context.Message.Username) }).ConfigureAwait(false);

            return;
        }

        _logger.LogTrace("Check if email address already exists");
        if (await EmailAddressExistsAsync(dbContext, context.Message.EmailAddress, context.CancellationToken).ConfigureAwait(false))
        {
            _logger.LogTrace("Email address already exists");

            if (context.RequestId.HasValue)
                await context.RespondAsync(new CreateUserFailed { EmailAddressAlreadyExists = new(context.Message.EmailAddress) }).ConfigureAwait(false);
            
            return;
        }

        _logger.Mapper().Mapping<CreateUser, User>();
        var user = _mapper.Map<User>(context.Message);
        user.HashedPassword = _passwordHashingService.ComputeHash(context.Message.Password);

        _logger.DbContext().AddingEntityLocally<User>();
        dbContext.Users.Add(user);

        _logger.DbContext().SavingChanges();
        await dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);

        _logger.Mapper().Mapping<User, UserCreated>();
        var @event = _mapper.Map<UserCreated>(user);
        
        if (context.RequestId.HasValue)
            await context.RespondAsync(@event).ConfigureAwait(false);

        _logger.Event().Publishing<UserCreated>();
        await context.Publish(@event).ConfigureAwait(false);
        
        _logger.DbContext().CommittingTransaction();
        await transaction.CommitAsync(context.CancellationToken).ConfigureAwait(false);
    }

    static async Task<bool> UsernameExistsAsync(UserDbContext context, string username, CancellationToken cancellationToken) => await context.Users
        .AsNoTracking()
        .Where(user => user.Username == username)
        .AnyAsync(cancellationToken)
        .ConfigureAwait(false);

    static async Task<bool> EmailAddressExistsAsync(UserDbContext context, string emailAddress, CancellationToken cancellationToken) => await context.Users
        .AsNoTracking()
        .Where(user => user.EmailAddress == emailAddress)
        .AnyAsync(cancellationToken)
        .ConfigureAwait(false);
}
