namespace UserOrchestration.Logging;

public sealed class LoggerForEvent
{
    public ILogger Logger { get; }

    internal LoggerForEvent(ILogger logger) => Logger = logger;

    public void Publishing(string @event) => Logger.LogTrace("Publish Event: {Event}", @event);

    public void Publishing<TEvent>() => Publishing(typeof(TEvent).Name);
}
