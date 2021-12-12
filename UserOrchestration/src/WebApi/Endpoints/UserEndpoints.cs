using System.Net.Mime;

namespace UserOrchestration.Endpoints;

static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/user/{id}", GetAsync)
            .WithTags("User")
            .Produces<GetUserResponse>(StatusCodes.Status200OK)
            .Produces<GetUserFailedResponse>(StatusCodes.Status400BadRequest);

        builder.MapPost("/user", CreateAsync)
            .WithTags("User")
            .Accepts<CreateUserRequest>(MediaTypeNames.Application.Json)
            .Produces<int>(StatusCodes.Status201Created)
            .Produces<CreateUserFailedResponse>(StatusCodes.Status400BadRequest);

        builder.MapPut("/user/{id}/activate", ActivateAsync)
            .WithTags("User")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ActivateUserFailedResponse>(StatusCodes.Status400BadRequest);

        builder.MapPut("/user/{id}/deactivate", DeactivateAsync)
            .WithTags("User")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<DeactivateUserFailedResponse>(StatusCodes.Status400BadRequest);

        builder.MapPut("/user/{id}/email/verify", VerifyEmailAsync)
            .WithTags("User")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<VerifyUserEmailFailedResponse>(StatusCodes.Status400BadRequest);

        return builder;
    }

    static async Task<IResult> GetAsync(ILogger<Program> logger, IMapper mapper, IRequestClient<GetUser> getUserClient, int id, CancellationToken cancellationToken)
    {
        logger.Http().Get("/user/{id}");

        var (user, getUserFailed) = await getUserClient.GetResponse<UserObj, GetUserFailed>(new GetUser(id), cancellationToken).ConfigureAwait(false);

        if (user.IsCompletedSuccessfully)
        {
            logger.LogDebug("User was found");
            var response = await user;
            return Results.Ok(mapper.Map<GetUserResponse>(response.Message));
        }

        if (getUserFailed.IsCompletedSuccessfully)
        {
            logger.LogDebug("Failed to get user");
            var response = await getUserFailed;
            return Results.BadRequest(mapper.Map<GetUserFailedResponse>(response.Message));
        }

        throw new UnsupportedResponseException();
    }

    static async Task<IResult> CreateAsync(ILogger<Program> logger, IMapper mapper, IRequestClient<CreateUser> createUserClient, CreateUserRequest request, CancellationToken cancellationToken)
    {
        logger.Http().Post("/user");
        var createUser = mapper.Map<CreateUser>(request);
        var (userCreated, createUserFailed) = await createUserClient.GetResponse<UserCreated, CreateUserFailed>(createUser, cancellationToken).ConfigureAwait(false);

        if (userCreated.IsCompletedSuccessfully)
        {
            logger.LogDebug("Create user succeeded");
            var response = await userCreated.ConfigureAwait(false);
            return Results.Created($"/user/{response.Message.Id}", response.Message.Id);
        }
        
        if (createUserFailed.IsCompletedSuccessfully)
        {
            logger.LogDebug("Create user failed");
            var response = await createUserFailed.ConfigureAwait(false);
            return Results.BadRequest(mapper.Map<CreateUserFailedResponse>(response.Message));
        }

        throw new UnsupportedResponseException();
    }

    static async Task<IResult> ActivateAsync(ILogger<Program> logger, IMapper mapper, IRequestClient<ActivateUser> activateUserClient, int id, CancellationToken cancellationToken)
    {
        logger.Http().Put("/user/{id}/activate");
        var (userActivated, activateUserFailed) = await activateUserClient.GetResponse<UserActivated, ActivateUserFailed>(new ActivateUser(id), cancellationToken).ConfigureAwait(false);

        if (userActivated.IsCompletedSuccessfully)
        {
            logger.LogDebug("Activate user succeeded");
            var response = await userActivated.ConfigureAwait(false);
            return Results.NoContent();
        }

        if (activateUserFailed.IsCompletedSuccessfully)
        {
            logger.LogDebug("Activate user failed");
            var response = await activateUserFailed.ConfigureAwait(false);
            return Results.BadRequest(mapper.Map<ActivateUserFailedResponse>(response.Message));
        }

        throw new UnsupportedResponseException();
    }

    static async Task<IResult> DeactivateAsync(ILogger<Program> logger, IMapper mapper, IRequestClient<DeactivateUser> deactivateUserClient, int id, CancellationToken cancellationToken)
    {
        logger.Http().Put("/user/{id}/deactivate");
        var (userDeactivated, deactivateUserFailed) = await deactivateUserClient.GetResponse<UserDeactivated, DeactivateUserFailed>(new DeactivateUser(id), cancellationToken).ConfigureAwait(false);

        if (userDeactivated.IsCompletedSuccessfully)
        {
            logger.LogDebug("Deactivate user succeeded");
            var response = await userDeactivated.ConfigureAwait(false);
            return Results.NoContent();
        }

        if (deactivateUserFailed.IsCompletedSuccessfully)
        {
            logger.LogDebug("Deactivate user failed");
            var response = await deactivateUserFailed.ConfigureAwait(false);
            return Results.BadRequest(mapper.Map<DeactivateUserFailedResponse>(response.Message));
        }

        throw new UnsupportedResponseException();
    }

    static async Task<IResult> VerifyEmailAsync(ILogger<Program> logger, IMapper mapper, IRequestClient<VerifyUserEmail> verifyUserEmailClient, int id, VerifyUserEmailRequest request, CancellationToken cancellationToken)
    {
        logger.Http().Put("/user/{id}/email/verify");
        var (userEmailVerified, verifyUserEmailFailed) = await verifyUserEmailClient.GetResponse<UserEmailVerified, VerifyUserEmailFailed>(new VerifyUserEmail(id, request.EmailAddress), cancellationToken).ConfigureAwait(false);

        if (userEmailVerified.IsCompletedSuccessfully)
        {
            logger.LogDebug("Verify user email succeeded");
            var response = await userEmailVerified.ConfigureAwait(false);
            return Results.NoContent();
        }

        if (verifyUserEmailFailed.IsCompletedSuccessfully)
        {
            logger.LogDebug("Verify user email failed");
            var response = await verifyUserEmailFailed.ConfigureAwait(false);
            return Results.BadRequest(mapper.Map<VerifyUserEmailFailedResponse>(response.Message));
        }

        throw new UnsupportedResponseException();
    }
}