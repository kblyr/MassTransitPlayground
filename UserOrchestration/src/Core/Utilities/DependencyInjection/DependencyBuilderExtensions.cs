using _Core = UserOrchestration.Core;

namespace UserOrchestration.Utilities.Core;

public static class DependencyBuilderExtensions
{
    public static _Core.DependencyBuilder OnUtilities(this _Core.DependencyBuilder builder, Action<DependencyBuilder>? configureBuilder = null)
    {
        var localBuilder = new DependencyBuilder(builder.Services);
        configureBuilder?.Invoke(localBuilder);
        return builder;
    }

    public static DependencyBuilder Defaults(this DependencyBuilder builder) => builder
        .AddUserPasswordHashingService();

    public static DependencyBuilder AddUserPasswordHashingService(this DependencyBuilder builder)
    {
        builder.Services.AddSingleton<IUserPasswordHashingService, UserPasswordHashingService>();
        return builder;
    }
}
