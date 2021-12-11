namespace UserOrchestration.Events;

public record UserDeactivated
{
    public int Id { get; init; }

    public UserDeactivated() { }

    public UserDeactivated(int id) => Id = id;
}