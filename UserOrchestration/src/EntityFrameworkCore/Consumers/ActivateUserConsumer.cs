namespace UserOrchestration.Consumers;

sealed class ActivateUserConsumer : IConsumer<ActivateUser>
{
    readonly ILogger<ActivateUserConsumer> _logger;
    readonly IDbContextFactory<UserDbContext> _contextFactory;

    public ActivateUserConsumer(ILogger<ActivateUserConsumer> logger, IDbContextFactory<UserDbContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task Consume(ConsumeContext<ActivateUser> context)
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
                await context.RespondAsync(new ActivateUserFailed { NotFound = new(context.Message.Id) }).ConfigureAwait(false);

            return;
        }

        _logger.LogTrace("Check if user is already active");
        if (user.IsActive)
        {
            _logger.LogTrace("User already is active");

            if (context.RequestId.HasValue)
                await context.RespondAsync(new ActivateUserFailed { AlreadyActivated = new(context.Message.Id) }).ConfigureAwait(false);

            return;
        }
        
        _logger.LogTrace("Activating user");
        user.IsActive = true;
        dbContext.Users.Update(user);

        _logger.DbContext().SavingChanges();
        await dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);

        var @event = new UserActivated(context.Message.Id);

        if (context.RequestId.HasValue)
            await context.RespondAsync(@event).ConfigureAwait(false);

        _logger.Event().Publishing<UserActivated>();
        await context.Publish(@event).ConfigureAwait(false);

        _logger.DbContext().CommittingTransaction();
        await transaction.CommitAsync(context.CancellationToken).ConfigureAwait(false);
    }
}
