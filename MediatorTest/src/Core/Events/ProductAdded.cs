namespace MediatorTest.Events;

public record ProductAdded
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
}