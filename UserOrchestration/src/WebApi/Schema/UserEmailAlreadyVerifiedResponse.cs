namespace UserOrchestration.Schema;

public record UserEmailAlreadyVerifiedResponse
{
    public int Id { get; init; }
    public string EmailAddress { get; init; } = "";
}