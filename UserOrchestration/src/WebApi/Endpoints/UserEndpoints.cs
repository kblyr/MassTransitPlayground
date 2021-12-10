using System.Net.Mime;

namespace UserOrchestration.Endpoints;

static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/user", CreateUserAsync)
            .WithTags("User")
            .Accepts<CreateUserRequest>(MediaTypeNames.Application.Json)
            .Produces<int>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return builder;
    }

    static async Task<IResult> CreateUserAsync(ILogger logger, IMapper mapper, IRequestClient<CreateUser> createUserClient, CreateUserRequest request, CancellationToken cancellationToken)
    {
        logger.LogDebug("POST /user");
        var createUser = mapper.Map<CreateUser>(request);
        var (userCreated, createUserFailed) = await createUserClient.GetResponse<UserCreated, ICreateUserFailed>(createUser, cancellationToken).ConfigureAwait(false);

        if (userCreated.IsCompletedSuccessfully)
        {
            logger.LogDebug("Create user succeeded");
            var response = await userCreated;
            return Results.Created($"/user/{response.Message.Id}", response.Message.Id);
        }
        
        if (createUserFailed.IsCompletedSuccessfully)
        {
            logger.LogDebug("Create user failed");
            var response = await createUserFailed;

            switch(response)
            {
                case UsernameAlreadyExists error:
                    logger.LogDebug("Username already exists");
                    return Results.BadRequest(mapper.Map<UsernameAlreadyExistsResponse>(error));
                case UserEmailAddressAlreadyExists error:
                    logger.LogDebug("User email address already exists");
                    return Results.BadRequest(mapper.Map<UserEmailAddressAlreadyExistsResponse>(error));
            }
        }

        throw new UnsupportedResponseException();
    }
}