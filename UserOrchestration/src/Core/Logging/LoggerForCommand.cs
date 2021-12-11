namespace UserOrchestration.Logging;

public sealed class LoggerForCommand
{
    public ILogger Logger { get; }

    internal LoggerForCommand(ILogger logger) => Logger = logger;

    public void Sending(string command) => Logger.LogTrace("Sending Command: {Command}", command);

    public void Sending<TCommand>() => Sending(typeof(TCommand).Name);
}