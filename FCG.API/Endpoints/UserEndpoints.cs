using FCG.Application.Modules.Users;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FCG.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app
            .MapGroup("/users")
            .WithTags("User");

        group.MapGet("/", GetAllUsers)
            .WithName("GetAllUsers");

        group.MapPost("/", CreateUser);

        group.MapGet("/{id:guid}", GetUserById)
            .WithName("GetUserById");
    }

    private static async Task<Ok<IEnumerable<UserResponse>>> GetAllUsers(
        IUserAppService userAppService,
        CancellationToken cancellationToken)
    {
        var users = await userAppService.GetAll(cancellationToken);
        return TypedResults.Ok(users);
    }

    private static async Task<CreatedAtRoute> CreateUser(
        CreateUserRequest request,
        IUserAppService service,
        CancellationToken cancellationToken)
    {
        Guid id = await service.CreateUserAsync(request, cancellationToken);
        return TypedResults.CreatedAtRoute(nameof(GetUserById), routeValues: new { id });
    }

    private static async Task<Results<Ok<UserResponse>, NotFound>> GetUserById(
        Guid id,
        IUserAppService service,
        CancellationToken cancellationToken)
    {
        UserResponse? user = await service.GetByIdAsync(id, cancellationToken);

        return user is not null
            ? TypedResults.Ok(user)
            : TypedResults.NotFound();
    }
}