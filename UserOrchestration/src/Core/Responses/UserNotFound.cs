namespace UserOrchestration.Responses;

public record UserNotFound : IGetUserFailed
{
    public int Id { get; init; }

    public UserNotFound() { }

    public UserNotFound(int id) => Id = id;
}
