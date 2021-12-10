using MassTransit;

static class MassTransitDependencyInjection
{
    public static IServiceCollection AddMassTransitForUserOrchestration(this IServiceCollection services)
    {
        services.AddMassTransit(massTransit => {
            massTransit.UsingRabbitMq((context, rabbitMq) => {
                
            });
        });

        return services;
    } 
}