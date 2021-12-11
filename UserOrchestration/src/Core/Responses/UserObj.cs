namespace UserOrchestration.Responses;

public record UserObj
{
    public int Id { get; init; }
    public string Username { get; init; } = "";
    public string EmailAddress { get; init; } = "";
    public bool IsActive { get; init; }
    public bool IsPasswordChangeRequired { get; init; }
    public bool IsEmailVerified { get; init; }
}
