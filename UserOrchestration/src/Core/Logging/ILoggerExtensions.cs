using System.Net.NetworkInformation;
namespace UserOrchestration.Logging;

public static class IloggerExtensions
{
    static LoggerForEvent? _event;
    static LoggerForMapper? _mapper;

    public static LoggerForEvent Event(this ILogger logger) => _event ??= new(logger);

    public static LoggerForMapper Mapper(this ILogger logger) => _mapper ??= new(logger);
}
