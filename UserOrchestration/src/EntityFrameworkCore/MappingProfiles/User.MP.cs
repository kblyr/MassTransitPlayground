namespace UserOrchestration.MappingProfiles;

sealed class UserMP : Profile
{
    public UserMP()
    {
        CreateMap<CreateUser, User>()
            .ForMember(user => user.HashedPassword, config => config.Ignore());
        CreateMap<UsernameAlreadyExists, CreateUserFailed>()
            .ForMember(dest => dest.UsernameAlreadyExists, config => config.MapFrom(src => src));
        CreateMap<UserEmailAddressAlreadyExists, CreateUserFailed>()
            .ForMember(dest => dest.EmailAddressAlreadyExists, config => config.MapFrom(src => src));

        CreateMap<UserNotFound, GetUserFailed>()
            .ForMember(dest => dest.NotFound, config => config.MapFrom(src => src));

        CreateMap<UserAlreadyActivated, ActivateUserFailed>()
            .ForMember(dest => dest.AlreadyActivated, config => config.MapFrom(src => src));
        CreateMap<UserNotFound, ActivateUserFailed>()
            .ForMember(dest => dest.NotFound, config => config.MapFrom(src => src));

        CreateMap<User, UserCreated>();
    }
}