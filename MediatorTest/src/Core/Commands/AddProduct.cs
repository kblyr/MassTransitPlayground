namespace MediatorTest.Commands;

public record AddProduct
{
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
}