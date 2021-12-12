namespace UserOrchestration.Events;

public record UserEmailVerified
{
    public int Id { get; init; }
    public string EmailAddress { get; init; } = "";
}