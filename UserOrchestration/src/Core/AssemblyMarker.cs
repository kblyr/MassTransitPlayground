using System.Reflection;

namespace UserOrchestration.Core;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}