namespace UserOrchestration.StateMachines;

sealed class UserEmailVerificationSM : MassTransitStateMachine<UserEmailVerificationSMI>
{
    readonly ILogger<UserEmailVerificationSM> _logger;
    readonly IMapper _mapper;

    public UserEmailVerificationSM(ILogger<UserEmailVerificationSM> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;

        InstanceState(instance => instance.CurrentState);
        ConfigureEvents();
        ConfigureEventActivities();
    }

    public State Pending { get; } = default!;
    public State Sent { get; private set; } = default!;

    public Event<UserCreated> UserCreated { get; private set; } = default!;
    public Event<UserEmailVerificationSent> UserEmailVerificationSent { get; private set; } = default!;
    public Event<UserEmailVerified> UserEmailVerified { get; private set; } = default!;

    void ConfigureEvents()
    {
        _logger.LogTrace("Configuring events");

        Event(() => UserCreated,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.Id && instance.EmailAddress == context.Message.EmailAddress)
                    .SelectId(context => NewId.NextGuid());
            }
        );

        Event(() => UserEmailVerificationSent,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.Id && instance.EmailAddress == context.Message.EmailAddress)
                    .SelectId(context => NewId.NextGuid());
            }
        );

        Event(() => UserEmailVerified,
            correlation => {
                correlation.CorrelateBy((instance, context) => instance.UserId == context.Message.Id && instance.EmailAddress == context.Message.EmailAddress);
                correlation.OnMissingInstance(instance => instance.Discard());
            }
        );
    }

    void ConfigureEventActivities()
    {
        _logger.LogTrace("Configuring event activities");

        Initially(
            Ignore(UserCreated, context => context.Data.IsEmailVerified == true),
            Ignore(UserEmailVerified),
            When(UserCreated, context => context.Data.IsEmailVerified == false)
                .Then(OnUserCreated)
                .Publish(context => _mapper.Map<SendUserEmailVerification>(context.Data))
                .TransitionTo(Pending),
            When(UserEmailVerificationSent)
                .Then(OnUserEmailVerificationSent)
                .TransitionTo(Sent)
        );

        During(Pending,
            Ignore(UserCreated),
            When(UserEmailVerificationSent)
                .Then(OnUserEmailVerificationSent)
                .TransitionTo(Sent),
            When(UserEmailVerified)
                .Then(OnUserEmailVerified)
                .Finalize()
        );

        During(Sent,
            Ignore(UserCreated),
            Ignore(UserEmailVerificationSent),
            When(UserEmailVerified)
                .Then(OnUserEmailVerified)
                .Finalize()
        );
    }

    void OnUserCreated(BehaviorContext<UserEmailVerificationSMI, UserCreated> context)
    {
        _logger.LogInformation("User was created");
        context.Instance.UserId = context.Data.Id;
        context.Instance.EmailAddress = context.Data.EmailAddress;
    }

    void OnUserEmailVerificationSent(BehaviorContext<UserEmailVerificationSMI, UserEmailVerificationSent> context)
    {
        _logger.LogInformation("User email verification was sent");
    }

    void OnUserEmailVerified(BehaviorContext<UserEmailVerificationSMI, UserEmailVerified> context)
    {
        _logger.LogInformation("User email has been verified");
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