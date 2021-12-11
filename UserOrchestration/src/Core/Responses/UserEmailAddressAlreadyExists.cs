namespace UserOrchestration.Responses;

public record UserEmailAddressAlreadyExists : ICreateUserFailed
{
    public string EmailAddress { get; init; } = "";
}