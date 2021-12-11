using System.Reflection;

namespace UserOrchestration.EntityFrameworkCore;

public static class AssemblyMarker
{
    public static Assembly Assembly = typeof(AssemblyMarker).Assembly;
}