using _Core = UserOrchestration.Core;

namespace UserOrchestration.EntityFrameworkCore;

public static class DependencyBuilderExtensions
{
    public static _Core.DependencyBuilder AddEntityFrameworkCore(this _Core.DependencyBuilder builder, Action<DependencyBuilder>? configureBuilder = null)
    {
        var localBuilder = new DependencyBuilder(builder.Services);
        configureBuilder?.Invoke(localBuilder);
        return builder;
    }
}