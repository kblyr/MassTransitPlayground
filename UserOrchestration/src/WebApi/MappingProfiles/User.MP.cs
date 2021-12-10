namespace UserOrchestration.MappingProfiles;

sealed class UserMP : Profile
{
    public UserMP()
    {
        CreateMap<CreateUser, CreateUserRequest>();
        CreateMap<UsernameAlreadyExists, UsernameAlreadyExistsResponse>();
        CreateMap<UserEmailAddressAlreadyExists, UserEmailAddressAlreadyExistsResponse>();
    }
}