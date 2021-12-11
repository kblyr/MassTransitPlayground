namespace UserOrchestration.Consumers.MailingService;

public static class IRegistrationConfiguratorExtensions
{
    public static void AddConsumersFromMailingService(this IRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<SendUserEmailVerificationConsumer>();
    }
}