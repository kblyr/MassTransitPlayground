namespace UserOrchestration.Schema;

public record DeactivateUserFailedResponse
{
    public UserNotFoundResponse? NotFound { get; init; }
    public UserAlreadyDeactivatedResponse? AlreadyDeactivated { get; init; }
}
