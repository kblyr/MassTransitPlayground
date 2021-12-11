namespace UserOrchestration.Commands;

public record SendUserEmailVerification
{
    public int Id { get; init; }
    public string EmailAddress { get; init; } = "";

    public SendUserEmailVerification() { }

    public SendUserEmailVerification(int id, string emailAddress) 
    {
        Id = id;
        EmailAddress = emailAddress;
    }
}