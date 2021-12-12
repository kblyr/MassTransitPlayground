namespace UserOrchestration.MappingProfiles;

sealed class UserMP : Profile
{
    public UserMP()
    {
        CreateMap<UserCreated, SendUserEmailVerification>();
        CreateMap<SendUserEmailVerification, UserEmailVerificationSent>();
        CreateMap<VerifyUserEmail, UserEmailVerified>();
        CreateMap<VerifyUserEmail, UserEmailNotFound>();
        CreateMap<VerifyUserEmail, UserEmailAlreadyVerified>();
        CreateMap<UserEmailNotFound, VerifyUserEmailFailed>()
            .ForMember(dest => dest.EmailNotFound, config => config.MapFrom(src => src));
        CreateMap<UserEmailAlreadyVerified, VerifyUserEmailFailed>()
            .ForMember(dest => dest.AlreadyVerified, config => config.MapFrom(src => src));
    }
}