namespace UserOrchestration.MappingProfiles;

sealed class UserMP : Profile
{
    public UserMP()
    {
        CreateMap<CreateUserRequest, CreateUser>();
        CreateMap<UsernameAlreadyExists, UsernameAlreadyExistsResponse>();
        CreateMap<UserEmailAddressAlreadyExists, UserEmailAddressAlreadyExistsResponse>();
        CreateMap<UsernameAlreadyExists, CreateUserFailedResponse>()
            .ForMember(dest => dest.UsernameAlreadyExists, config => config.MapFrom(src => src));
        CreateMap<UserEmailAddressAlreadyExists, CreateUserFailedResponse>()
            .ForMember(dest => dest.EmailAddressAlreadyExists, config => config.MapFrom(src => src));
        CreateMap<UserObj, GetUserResponse>();
        CreateMap<UserNotFound, UserNotFoundResponse>();
        CreateMap<UserNotFound, GetUserFailedResponse>()
            .ForMember(dest => dest.NotFound, config => config.MapFrom(src => src));
    }
}