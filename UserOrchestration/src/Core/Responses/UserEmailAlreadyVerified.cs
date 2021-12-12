namespace UserOrchestration.Responses;

public record UserEmailAlreadyVerified
{
    public int Id { get; init; }
    public string EmailAddress { get; init; } = "";
}