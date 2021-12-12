namespace UserOrchestration.Schema;

public record UserEmailNotFoundResponse
{
    public int Id { get; init; }
    public string EmailAddress { get; init; } = "";
}
