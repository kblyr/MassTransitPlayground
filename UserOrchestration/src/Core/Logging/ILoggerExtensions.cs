using System.Net.NetworkInformation;
namespace UserOrchestration.Logging;

public static class IloggerExtensions
{
    static LoggerForCommand? _command;
    static LoggerForEvent? _event;
    static LoggerForMapper? _mapper;

    public static LoggerForCommand Command(this ILogger logger) => _command ??= new(logger);

    public static LoggerForEvent Event(this ILogger logger) => _event ??= new(logger);

    public static LoggerForMapper Mapper(this ILogger logger) => _mapper ??= new(logger);
}
