namespace UserOrchestration.Responses;

public record UserEmailNotFound
{
    public int Id { get; init; }
    public string EmailAddress { get; init; } = "";
}
