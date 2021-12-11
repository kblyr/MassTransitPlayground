namespace UserOrchestration.Core;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddUserOrchestration(this IServiceCollection services, Action<DependencyBuilder>? configureBuilder = null)
    {
        var builder = new DependencyBuilder(services);
        configureBuilder?.Invoke(builder);
        return services;
    }
}