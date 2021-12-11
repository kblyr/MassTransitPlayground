namespace UserOrchestration.Core;

public class DependencyBuilder
{
    public IServiceCollection Services { get; }
    
    public DependencyBuilder(IServiceCollection services)
    {
        Services = services;
    }
}