namespace UserOrchestration.MappingProfiles;

sealed class UserMP : Profile
{
    public UserMP()
    {
        CreateMap<UserCreated, SendUserEmailVerification>();
        CreateMap<SendUserEmailVerification, UserEmailVerificationSent>();
    }
}