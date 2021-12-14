using System.Reflection;

namespace MediatorTest.EntityFrameworkCore;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly; 
}