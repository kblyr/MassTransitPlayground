namespace UserOrchestration.Responses;

public record GetUserFailed
{
    public UserNotFound? NotFound { get; init; }
}