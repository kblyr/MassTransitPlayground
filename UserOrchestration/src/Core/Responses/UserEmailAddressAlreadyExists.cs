namespace UserOrchestration.Responses;

public record UserEmailAddressAlreadyExists
{
    public string EmailAddress { get; init; } = "";

    public UserEmailAddressAlreadyExists() { }

    public UserEmailAddressAlreadyExists(string emailAddress) => EmailAddress = emailAddress;
}