namespace UserOrchestration.Schema;

public record GetUserFailedResponse
{
    public UserNotFoundResponse? NotFound { get; init; }
}