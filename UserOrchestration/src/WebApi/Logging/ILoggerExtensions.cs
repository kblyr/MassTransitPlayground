namespace UserOrchestration.Logging;

public static class ILoggerExtensions
{
    static LoggerForHttp? _http;

    public static LoggerForHttp Http(this ILogger logger) => _http ??= new(logger);
}