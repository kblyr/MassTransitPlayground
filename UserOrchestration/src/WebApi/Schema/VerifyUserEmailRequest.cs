namespace UserOrchestration.Schema;

public record VerifyUserEmailRequest
{
    public string EmailAddress { get; init; } = "";
}

public record VerifyUserEmailFailedResponse
{
    public UserEmailNotFoundResponse? EmailNotFound { get; init; }
    public UserEmailAlreadyVerified? AlreadyVerified { get; init; }    
}
