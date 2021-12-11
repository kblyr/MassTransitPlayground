namespace UserOrchestration.Data.EntityFrameworkCore;

public static class DependencyBuilderExtensions
{
    public static DependencyBuilder AddUserDbContextFactory(this DependencyBuilder builder, Action<DbContextOptionsBuilder>? configureOptions = null, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        builder.Services.AddDbContextFactory<UserDbContext>(configureOptions, lifetime);
        return builder;
    }

}