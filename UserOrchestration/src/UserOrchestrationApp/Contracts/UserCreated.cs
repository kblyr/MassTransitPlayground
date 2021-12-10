namespace UserOrchestration.Contracts;

public record UserCreated
{
    public int Id { get; init; }
    public string Username { get; init; } = "";
    public string EmailAddress { get; init; } = "";
    public bool IsActive { get; init; }
    public bool IsEmailVerified { get; init; }

    public UserCreated() { }

    public UserCreated(int id, CreateUser source)
    {
        Id = id;
        Username = source.Username;
        EmailAddress = source.EmailAddress;
        IsActive = source.IsActive;
        IsEmailVerified = source.IsEmailVerified;
    }
}