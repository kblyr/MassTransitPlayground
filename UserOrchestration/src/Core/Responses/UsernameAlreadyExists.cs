namespace UserOrchestration.Responses;

public record UsernameAlreadyExists : ICreateUserFailed
{
    public string Username { get; init; } = "";
}
