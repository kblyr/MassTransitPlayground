namespace UserOrchestration.Schema;

public record ActivateUserFailedResponse
{
    public UserNotFoundResponse? NotFound { get; init; }
    public UserAlreadyActivatedResponse? AlreadyActivated { get; init; }
}