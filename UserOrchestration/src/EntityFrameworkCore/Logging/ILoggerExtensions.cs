namespace UserOrchestration.Logging;

public static class ILoggerExtensions
{
    static LoggerForDbContext? _dbContext;
    public static LoggerForDbContext DbContext(this ILogger logger) => _dbContext ??= new(logger);
}
