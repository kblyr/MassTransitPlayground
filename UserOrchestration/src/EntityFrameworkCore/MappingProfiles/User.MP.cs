namespace UserOrchestration.MappingProfiles;

sealed class UserMP : Profile
{
    public UserMP()
    {
        CreateMap<CreateUser, User>()
            .ForMember(user => user.HashedPassword, config => config.Ignore());

        CreateMap<User, UserCreated>();
    }
}