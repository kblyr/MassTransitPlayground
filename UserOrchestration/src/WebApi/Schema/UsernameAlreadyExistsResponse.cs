namespace UserOrchestration.Schema;

public record UsernameAlreadyExistsResponse
{
    public string Username { get; init; } = "";
}
