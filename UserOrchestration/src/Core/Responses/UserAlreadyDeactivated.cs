namespace UserOrchestration.Responses;

public record UserAlreadyDeactivated
{
    public int Id { get; init; }

    public UserAlreadyDeactivated() { }

    public UserAlreadyDeactivated(int id) => Id = id;
}