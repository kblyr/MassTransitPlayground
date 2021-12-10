namespace UserOrchestration.Responses;

public record UsernameAlreadyExists
{
    public string Username { get; init; } = "";
}
