namespace UserOrchestration.Consumers;

sealed class VerifyUserEmailConsumer : IConsumer<VerifyUserEmail>
{
    readonly ILogger<VerifyUserEmailConsumer> _logger;
    readonly IDbContextFactory<UserDbContext> _contextFactory;
    readonly IMapper _mapper;

    public VerifyUserEmailConsumer(ILogger<VerifyUserEmailConsumer> logger, IDbContextFactory<UserDbContext> contextFactory, IMapper mapper)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<VerifyUserEmail> context)
    {
        _logger.DbContext().Creating<UserDbContext>();
        using var dbContext = await _contextFactory.CreateDbContextAsync(context.CancellationToken).ConfigureAwait(false);
        using var transaction = await dbContext.Database.BeginTransactionAsync(context.CancellationToken).ConfigureAwait(false);

        _logger.LogTrace("Get by id");
        var user = await dbContext.Users
            .AsNoTracking()
            .Where(user => user.Id == context.Message.Id)
            .Where(user => user.EmailAddress == context.Message.EmailAddress)
            .FirstOrDefaultAsync(context.CancellationToken)
            .ConfigureAwait(false);

        _logger.LogTrace("Check if user exists");
        if (user is null)
        {
            _logger.LogTrace("User does not exists");

            if (context.RequestId.HasValue)
            {
                var response = _mapper.Map<VerifyUserEmailFailed>(_mapper.Map<UserEmailNotFound>(context.Message));
                await context.RespondAsync(response).ConfigureAwait(false);
            }

            return;
        }

        _logger.LogTrace("Check if user email is already verified");
        if (user.IsEmailVerified)
        {
            _logger.LogTrace("User email is already verified");

            if (context.RequestId.HasValue)
            {
                var response = _mapper.Map<VerifyUserEmailFailed>(_mapper.Map<UserEmailAlreadyVerified>(context.Message));
                await context.RespondAsync(response).ConfigureAwait(false);
            }

            return;
        }

        _logger.LogTrace("Verifying user email");
        user.IsEmailVerified = true;
        dbContext.Users.Update(user);

        _logger.DbContext().SavingChanges();
        await dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);

        var @event = _mapper.Map<UserEmailVerified>(context.Message);

        if (context.RequestId.HasValue)
            await context.RespondAsync(@event).ConfigureAwait(false);

        _logger.Event().Publishing<UserEmailVerified>();
        await context.Publish(@event).ConfigureAwait(false);

        _logger.DbContext().CommittingTransaction();
        await transaction.CommitAsync(context.CancellationToken).ConfigureAwait(false);
    }
}