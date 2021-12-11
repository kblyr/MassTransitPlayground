namespace UserOrchestration.Responses;

public record UsernameAlreadyExists
{
    public string Username { get; init; } = "";

    public UsernameAlreadyExists() { }

    public UsernameAlreadyExists(string username) => Username = username;
}
