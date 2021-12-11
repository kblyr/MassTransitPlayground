namespace UserOrchestration.Logging;

public sealed class LoggerForHttp
{
    public ILogger Logger { get; }

    internal LoggerForHttp(ILogger logger) => Logger = logger;

    public void Get(string routeTemplate) => Logger.LogTrace("GET: {RouteTemplate}", routeTemplate);

    public void Post(string routeTemplate) => Logger.LogTrace("POST: {RouteTemplate}", routeTemplate);
}