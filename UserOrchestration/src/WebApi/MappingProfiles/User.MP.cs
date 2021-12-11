namespace UserOrchestration.MappingProfiles;

sealed class UserMP : Profile
{
    public UserMP()
    {
        CreateMap<UserNotFound, UserNotFoundResponse>();
        CreateMap<UsernameAlreadyExists, UsernameAlreadyExistsResponse>();
        CreateMap<UserEmailAddressAlreadyExists, UserEmailAddressAlreadyExistsResponse>();
        CreateMap<UserAlreadyActivated, UserAlreadyActivatedResponse>();

        CreateMap<CreateUserRequest, CreateUser>();
        CreateMap<UsernameAlreadyExists, CreateUserFailedResponse>()
            .ForMember(dest => dest.UsernameAlreadyExists, config => config.MapFrom(src => src));
        CreateMap<UserEmailAddressAlreadyExists, CreateUserFailedResponse>()
            .ForMember(dest => dest.EmailAddressAlreadyExists, config => config.MapFrom(src => src));

        CreateMap<UserObj, GetUserResponse>();
        CreateMap<UserNotFound, GetUserFailedResponse>()
            .ForMember(dest => dest.NotFound, config => config.MapFrom(src => src));

        CreateMap<UserNotFound, ActivateUserFailedResponse>()
            .ForMember(dest => dest.NotFound, config => config.MapFrom(src => src));
        CreateMap<UserAlreadyActivated, ActivateUserFailedResponse>()
            .ForMember(dest => dest.AlreadyActivated, config => config.MapFrom(src => src));
    }
}