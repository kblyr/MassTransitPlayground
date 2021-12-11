namespace UserOrchestration.Consumers.EntityFrameworkCore;

public static class IRegistrationConfiguratorExtensions
{
    public static void AddConsumersFromEntityFrameworkCore(this IRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<ActivateUserConsumer>();
        configurator.AddConsumer<CreateUserConsumer>();
        configurator.AddConsumer<GetUserConsumer>();
    }
}