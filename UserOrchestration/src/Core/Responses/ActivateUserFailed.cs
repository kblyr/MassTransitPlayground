namespace UserOrchestration.Responses;

public record ActivateUserFailed
{
    public UserNotFound? NotFound { get; init; }
    public UserAlreadyActivated? AlreadyActivated { get; init; }
}
