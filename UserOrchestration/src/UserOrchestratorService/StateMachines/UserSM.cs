namespace UserOrchestration.StateMachines;

public sealed class UserSM : MassTransitStateMachine<UserSMI>
{
    ILogger<UserSM> _logger;
    public UserSM(ILogger<UserSM> logger)
    {
        _logger = logger;

        InstanceState(instance => instance.CurrentState);
        ConfigureEvents();
        ConfigureEventActivities();
    }

    public State Active { get; private set; } = default!;
    public State Inactive { get; private set; } = default!;

    public Event<UserCreated> UserCreated { get; private set; } = default!;

    void ConfigureEvents()
    {
        _logger.LogTrace("Configuring events");

        Event(() => UserCreated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.Id)
                    .SelectId(context => NewId.NextGuid());
            }
        );
    }

    void ConfigureEventActivities()
    {
        _logger.LogTrace("Configuring event activities");

        Initially(
            When(UserCreated, context => context.Data.IsActive == true)
                .Then(OnUserCreated)
                .TransitionTo(Active),
            When(UserCreated, context => context.Data.IsActive == false)
                .Then(OnUserCreated)
                .TransitionTo(Inactive)
        );
    }

    void OnUserCreated(BehaviorContext<UserSMI, UserCreated> context)
    {
        _logger.LogTrace("User was created");
        context.Instance.UserId = context.Data.Id;
        context.Instance.IsActive = context.Data.IsActive;
    }
}

public record UserSMI : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; } = default!;
    
    public int UserId { get; set; }
    public bool IsActive { get; set; }
}