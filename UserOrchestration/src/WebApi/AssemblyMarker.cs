using System.Reflection;

namespace UserOrchestration.WebApi;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}