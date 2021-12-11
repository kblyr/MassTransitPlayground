namespace UserOrchestration.Events;

public record UserActivated
{
    public int Id { get; init; }

    public UserActivated() { }

    public UserActivated(int id) => Id = id;
}
