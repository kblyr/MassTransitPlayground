namespace UserOrchestration.Schema;

public record UserEmailAddressAlreadyExistsResponse
{
    public string EmailAddress { get; init; } = "";
}