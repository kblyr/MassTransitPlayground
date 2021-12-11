namespace UserOrchestration.Events;

public record UserEmailVerificationSent
{
    public int Id { get; init; }
    public string EmailAddress { get; init; } = "";
}