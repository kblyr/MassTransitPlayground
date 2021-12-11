namespace UserOrchestration.Commands;

public record ActivateUser
{
    public int Id { get; init; }

    public ActivateUser() { }

    public ActivateUser(int id) => Id = id;
}
