namespace UserOrchestration.Commands;

public record VerifyUserEmail
{
    public int Id { get; init; }
    public string EmailAddress { get; init; } = "";

    public VerifyUserEmail() { }

    public VerifyUserEmail(int id, string emailAddress)
    {
        Id = id;
        EmailAddress = emailAddress;
    }
}