namespace UserOrchestration.Schema;

public record CreateUserFailedResponse
{
    public UserEmailAddressAlreadyExistsResponse? EmailAddressAlreadyExists { get; init; }
    public UsernameAlreadyExistsResponse? UsernameAlreadyExists { get; init; }
}
