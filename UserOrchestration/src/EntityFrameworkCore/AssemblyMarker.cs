using System.Reflection;

namespace UserOrchestration.EntityFrameworkCore;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}