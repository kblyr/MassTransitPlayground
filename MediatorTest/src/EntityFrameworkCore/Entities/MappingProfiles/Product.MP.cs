namespace MediatorTest.Entities.MappingProfiles;

sealed class Product_MP : Profile
{
    public Product_MP()
    {
        CreateMap<AddProduct, Product>();
    }
}