namespace UserOrchestration.Responses;

public record VerifyUserEmailFailed
{
    public UserEmailNotFound? EmailNotFound { get; init; }
    public UserEmailAlreadyVerified? AlreadyVerified { get; init; }
}
