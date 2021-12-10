namespace UserOrchestration.Responses;

public record UserEmailAddressAlreadyExists
{
    public string EmailAddress { get; init; } = "";
}