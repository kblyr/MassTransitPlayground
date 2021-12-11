namespace UserOrchestration.StateMachines;

sealed class UserEmailVerificationSM : MassTransitStateMachine<UserEmailVerificationSMI>
{
    readonly ILogger<UserEmailVerificationSM> _logger;

    public UserEmailVerificationSM(ILogger<UserEmailVerificationSM> logger)
    {
        _logger = logger;

        InstanceState(instance => instance.CurrentState);
    }

    public State Pending { get; }

    public Event<UserCreated> UserCreated { get; private set; } = default!;

    void ConfigureEvents()
    {
        _logger.LogTrace("Configuring events");

        Event(() => UserCreated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.Id && instance.EmailAddress == context.Message.EmailAddress)
                    .SelectId(context => NewId.NextGuid());
            }
        );
    }

    void ConfigureEventActivities()
    {
        _logger.LogTrace("Configuring event activities");

        Initially(
            Ignore(UserCreated, context => context.Data.IsEmailVerified == true),
            When(UserCreated, context => context.Data.IsEmailVerified == false)
                .Then(OnUserCreated)
                .TransitionTo(Pending)
        );
    }

    void OnUserCreated(BehaviorContext<UserEmailVerificationSMI, UserCreated> context)
    {
        _logger.LogTrace("User was created");
        context.Instance.UserId = context.Data.Id;
        context.Instance.EmailAddress = context.Data.EmailAddress;
    }
}

record UserEmailVerificationSMI : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; } = default!;

    public int UserId { get; set; }
    public string EmailAddress { get; set; } = "";
}