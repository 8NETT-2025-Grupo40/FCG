using FCG.Application.Modules.Login;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FCG.API.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app
            .MapGroup("/authentication")
            .WithTags("Authentication");

        group.MapPost("/Login", Login);
    }

    internal static async Task<Results<Ok<LoginResponse>, UnauthorizedHttpResult>> Login(
        ILoginAppServices loginAppServices,
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        LoginResponse result = await loginAppServices.LoginAppAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return TypedResults.Unauthorized();

        return TypedResults.Ok(result);
    }
}