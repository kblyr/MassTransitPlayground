namespace UserOrchestration.Consumers;

sealed class DeactivateUserConsumer : IConsumer<DeactivateUser>
{
    readonly ILogger<DeactivateUserConsumer> _logger;
    readonly IDbContextFactory<UserDbContext> _contextFactory;
    readonly IMapper _mapper;

    public DeactivateUserConsumer(ILogger<DeactivateUserConsumer> logger, IDbContextFactory<UserDbContext> contextFactory, IMapper mapper)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<DeactivateUser> context)
    {
        _logger.DbContext().Creating<UserDbContext>();
        using var dbContext = await _contextFactory.CreateDbContextAsync(context.CancellationToken).ConfigureAwait(false);
        using var transaction = await dbContext.Database.BeginTransactionAsync(context.CancellationToken).ConfigureAwait(false);

        _logger.LogTrace("Get by id");
        var user = await dbContext.Users
            .AsNoTracking()
            .Where(user => user.Id == context.Message.Id)
            .FirstOrDefaultAsync(context.CancellationToken)
            .ConfigureAwait(false);

        _logger.LogTrace("Check if user exists");
        if (user is null)
        {
            _logger.LogTrace("User does not exists");

            if (context.RequestId.HasValue)
            {
                var response = _mapper.Map<DeactivateUserFailed>(new UserNotFound(context.Message.Id));
                await context.RespondAsync(response).ConfigureAwait(false);
            }

            return;
        }

        _logger.LogTrace("Check if user is already inactive");
        if (!user.IsActive)
        {
            _logger.LogTrace("User is already inactive");

            if (context.RequestId.HasValue)
            {
                var response = _mapper.Map<DeactivateUserFailed>(new UserAlreadyDeactivated(context.Message.Id));
                await context.RespondAsync(response).ConfigureAwait(false);
            }

            return;
        }

        _logger.LogTrace("Deactivating user");
        user.IsActive = false;
        dbContext.Users.Update(user);

        _logger.DbContext().SavingChanges();
        await dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);

        var @event = new UserDeactivated(context.Message.Id);

        if (context.RequestId.HasValue)
            await context.RespondAsync(@event).ConfigureAwait(false);

        _logger.Event().Publishing<UserDeactivated>();
        await context.Publish(@event).ConfigureAwait(false);

        _logger.DbContext().CommittingTransaction();
        await transaction.CommitAsync(context.CancellationToken).ConfigureAwait(false);
    }
}
