sealed class GetUserConsumer : IConsumer<GetUser>
{
    readonly ILogger<GetUserConsumer> _logger;
    readonly IDbContextFactory<UserDbContext> _contextFactory;

    public GetUserConsumer(ILogger<GetUserConsumer> logger, IDbContextFactory<UserDbContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task Consume(ConsumeContext<GetUser> context)
    {
        _logger.DbContext().Creating<UserDbContext>();
        using var dbContext = await _contextFactory.CreateDbContextAsync(context.CancellationToken).ConfigureAwait(false);

        var user = await dbContext.Users
            .AsNoTracking()
            .Where(user => user.Id == context.Message.Id)
            .Select(user => new UserObj
            {
                Id = user.Id,
                Username = user.Username,
                EmailAddress = user.EmailAddress,
                IsActive = user.IsActive,
                IsPasswordChangeRequired = user.IsPasswordChangeRequired,
                IsEmailVerified = user.IsEmailVerified
            })
            .FirstOrDefaultAsync(context.CancellationToken)
            .ConfigureAwait(false);

        if (context.RequestId.HasValue)
        {
            if (user is null)
            {
                _logger.LogTrace("User does not exists");
                await context.RespondAsync(new GetUserFailed { NotFound = new(context.Message.Id) }).ConfigureAwait(false);
                return;
            }

            await context.RespondAsync(user).ConfigureAwait(false);
        }
    }
}