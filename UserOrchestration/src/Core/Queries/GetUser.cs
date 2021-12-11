namespace UserOrchestration.Queries;

public record GetUser
{
    public int Id { get; init; }

    public GetUser() { }

    public GetUser(int id) => Id = id;
}