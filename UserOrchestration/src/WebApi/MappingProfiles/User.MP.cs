namespace UserOrchestration.MappingProfiles;

sealed class UserMP : Profile
{
    public UserMP()
    {
        CreateMap<UserNotFound, UserNotFoundResponse>();
        CreateMap<UsernameAlreadyExists, UsernameAlreadyExistsResponse>();
        CreateMap<UserEmailAddressAlreadyExists, UserEmailAddressAlreadyExistsResponse>();
        CreateMap<UserAlreadyActivated, UserAlreadyActivatedResponse>();
        CreateMap<UserAlreadyDeactivated, UserAlreadyDeactivatedResponse>();
        CreateMap<CreateUserRequest, CreateUser>();
        CreateMap<CreateUserFailed, CreateUserFailedResponse>();
        CreateMap<UserObj, GetUserResponse>();
        CreateMap<GetUserFailed, GetUserFailedResponse>();
        CreateMap<ActivateUserFailed, ActivateUserFailedResponse>();
        CreateMap<DeactivateUserFailed, DeactivateUserFailedResponse>();
    }
}