namespace UserOrchestration.Responses;

public record UsernameAlreadyExists : ICreateUserFailed
{
    public string Username { get; init; } = "";

    public UsernameAlreadyExists() { }

    public UsernameAlreadyExists(string username) => Username = username;
}
