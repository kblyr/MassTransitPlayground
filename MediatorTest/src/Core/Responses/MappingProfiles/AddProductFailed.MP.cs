namespace MediatorTest.Responses.MappingProfiles;

sealed class AddProductFailed_MP : Profile
{
    public AddProductFailed_MP()
    {
        CreateMap<ProductNameAlreadyExists, AddProductFailed>()
            .ForMember(dest => dest.NameAlreadyExists, config => config.MapFrom(src => src));
    }
}