namespace UserOrchestration.Logging;

public sealed class LoggerForDbContext
{
    public ILogger Logger { get; }

    internal LoggerForDbContext(ILogger logger) => Logger = logger;

    public void Creating(string dbContext) => Logger.LogTrace("Create DbContext: {DbContext}", dbContext);

    public void Creating<TDbContext>() => Creating(typeof(TDbContext).Name);

    public void CommittingTransaction() => Logger.LogTrace("Committing transaction");

    public void SavingChanges() => Logger.LogTrace("Saving changes to database");

    public void AddingEntityLocally(string entity) => Logger.LogTrace("Adding {Entity} locally", entity);

    public void AddingEntityLocally<TEntity>() => AddingEntityLocally(typeof(TEntity).Name);
}