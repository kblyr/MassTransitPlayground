namespace UserOrchestration.Commands;

public record DeactivateUser
{
    public int Id { get; init; }

    public DeactivateUser() { }

    public DeactivateUser(int id) => Id = id;
}