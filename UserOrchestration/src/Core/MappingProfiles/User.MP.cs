namespace UserOrchestration.MappingProfiles;

sealed class UserMP : Profile
{
    public UserMP()
    {
        CreateMap<SendUserEmailVerification, UserEmailVerificationSent>();
    }
}