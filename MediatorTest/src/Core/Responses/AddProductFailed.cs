namespace MediatorTest.Responses;

public record AddProductFailed
{
    public ProductNameAlreadyExists? NameAlreadyExists { get; init; }
}