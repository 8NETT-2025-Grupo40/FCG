using FCG.API.Setup;
using FCG.Application.Modules.Users;
using FCG.Domain.Modules.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FCG.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder userGroup = app
            .MapGroup("/users")
            .WithTags("User");

        userGroup.MapPost("/", CreateUser)
            .WithSummary("Register an standard user");

        userGroup.MapGet("/{id:guid}", GetUserById)
            .WithName("GetUserById")
            .RequireAuthorization();

        RouteGroupBuilder adminGroup = userGroup
            .MapGroup("/admin")
            .RequireAuthorization(PolicyNames.AdminOnly)
            // Hoje não existe muitos endpoints exclusivos de User para role Admin, mas o dia que existir, considerar colocar WithTags("Admin")
            .WithTags("User");

        adminGroup.MapGet("/", GetAllUsers)
            .WithName("GetAllUsers")
            .WithSummary("Lists all registered users.");

        adminGroup.MapPost("/", CreateAdminUser)
            .WithName("CreateAdminUser")
            .WithSummary("Register an administrator user");
    }

    private static async Task<Ok<IEnumerable<UserResponse>>> GetAllUsers(
        IUserAppService userAppService,
        CancellationToken cancellationToken)
    {
        var users = await userAppService.GetAll(cancellationToken);
        return TypedResults.Ok(users);
    }

    private static async Task<CreatedAtRoute> CreateUser(
        [FromBody] CreateUserRequest request,
        IUserAppService service,
        CancellationToken cancellationToken)
    {
        Guid id = await service.CreateUserAsync(request, UserRole.StandardUser, cancellationToken);
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

    private static async Task<CreatedAtRoute> CreateAdminUser(
        [FromBody] CreateUserRequest request,
        IUserAppService service,
        CancellationToken cancellationToken)
    {
        Guid id = await service.CreateUserAsync(request, UserRole.Admin, cancellationToken);
        return TypedResults.CreatedAtRoute(nameof(GetUserById), new { id });
    }
}
