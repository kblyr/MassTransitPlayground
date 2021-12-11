namespace UserOrchestration.Responses;

public record DeactivateUserFailed
{
    public UserNotFound? NotFound { get; init; }
    public UserAlreadyDeactivated? AlreadyDeactivated { get; init; }
}
