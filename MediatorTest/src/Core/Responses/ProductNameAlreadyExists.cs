namespace MediatorTest.Responses;

public record ProductNameAlreadyExists
{
    public string Name { get; init; } = "";
}
