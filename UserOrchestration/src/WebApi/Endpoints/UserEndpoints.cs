using System.Net.Mime;

namespace UserOrchestration.Endpoints;

static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/user/{id}", GetAsync)
            .WithTags("User")
            .Produces<GetUserResponse>(StatusCodes.Status200OK)
            .Produces<GetUserFailedResponse>(StatusCodes.Status404NotFound);

        builder.MapPost("/user", CreateAsync)
            .WithTags("User")
            .Accepts<CreateUserRequest>(MediaTypeNames.Application.Json)
            .Produces<int>(StatusCodes.Status201Created)
            .Produces<CreateUserFailedResponse>(StatusCodes.Status400BadRequest);

        builder.MapPut("/user/{id}/activate", ActivateAsync)
            .WithTags("User")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ActivateUserFailed>(StatusCodes.Status404NotFound)
            .Produces<ActivateUserFailed>(StatusCodes.Status400BadRequest);

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

            if (response.Message.NotFound is not null)
            {
                logger.LogDebug("User was not found");
                return Results.NotFound(mapper.Map<UserNotFoundResponse>(response.Message.NotFound));
            }
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

            if (response.Message.UsernameAlreadyExists is not null)
            {
                logger.LogDebug("Username already exists");
                return Results.BadRequest(mapper.Map<CreateUserFailedResponse>(response.Message.UsernameAlreadyExists));
            }

            if (response.Message.EmailAddressAlreadyExists is not null)
            {
                logger.LogDebug("User email address already exists");
                return Results.BadRequest(mapper.Map<CreateUserFailedResponse>(response.Message.EmailAddressAlreadyExists));
            }
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

            if (response.Message.NotFound is not null)
            {
                logger.LogDebug("User was not found");
                return Results.NotFound(mapper.Map<ActivateUserFailedResponse>(response.Message.NotFound));
            }

            if (response.Message.AlreadyActivated is not null)
            {
                logger.LogDebug("User is already activated");
                return Results.BadRequest(mapper.Map<ActivateUserFailedResponse>(response.Message.AlreadyActivated));
            }
        }

        throw new UnsupportedResponseException();
    }
}