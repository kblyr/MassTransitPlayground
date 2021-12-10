namespace UserOrchestration.Commands;

public record CreateUser
{
    public string Username { get; init; } = "";
    public string Password { get; init; } = "";
    public string EmailAddress { get; init; } = "";
    public bool IsActive { get; init; }
    public bool IsPasswordChangeRequired { get; init; }
    public bool IsEmailVerified { get; init; }
}