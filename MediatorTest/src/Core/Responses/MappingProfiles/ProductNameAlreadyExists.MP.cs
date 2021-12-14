namespace MediatorTest.Responses.MappingProfiles;

sealed class ProductNameAlreadyExists_MP : Profile
{
    public ProductNameAlreadyExists_MP()
    {
        CreateMap<AddProduct, ProductNameAlreadyExists>();
    }
}