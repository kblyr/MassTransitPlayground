namespace UserOrchestration.Responses;

public record UserAlreadyActivated
{
    public int Id { get; init; }

    public UserAlreadyActivated() { }

    public UserAlreadyActivated(int id) => Id = id;
}