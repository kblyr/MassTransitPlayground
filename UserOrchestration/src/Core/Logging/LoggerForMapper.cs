namespace UserOrchestration.Logging;

public sealed class LoggerForMapper
{
    public ILogger Logger { get; }

    internal LoggerForMapper(ILogger logger) => Logger = logger;

    public void Mapping(string source, string destination) => Logger.LogTrace("Mapping from {MapSource} to {MapDestination}", source, destination);

    public void Mapping<TSource, TDestination>() => Mapping(typeof(TSource).Name, typeof(TDestination).Name);
}