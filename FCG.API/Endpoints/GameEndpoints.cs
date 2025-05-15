using FCG.API.Setup;
using FCG.Application.Modules.Games;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FCG.API.Endpoints
{
    public static  class GameEndpoints
    {
        public static void MapGameEndpoints(this IEndpointRouteBuilder app)
        {
            // TODO: Os endpoints ainda não foram implementados
            var gamesGroup = app
                .MapGroup("/games")
                .WithTags("Game");

            gamesGroup.MapGet("/me", GetMyLibrary)
                .WithSummary("ATTENTION: In development - Retrieve authenticated user’s game library")
                .RequireAuthorization(PolicyNames.UserOnly);

            gamesGroup.MapGet("/{id:guid}", GetGameById)
                .WithName("GetGameById")
                .WithSummary("ATTENTION: In development - Create a new game")
                .RequireAuthorization(PolicyNames.AdminOnly);

            var adminGames = gamesGroup
                .MapGroup("/admin")
                // Hoje não existe muitos endpoints exclusivos de Game para role Admin, mas o dia que existir, considerar colocar WithTags("Admin")
                .WithTags("Game")
                .RequireAuthorization(PolicyNames.AdminOnly);

            adminGames.MapPost("/", CreateGame)
                .WithSummary("ATTENTION: In development - Create a new game");
        }

        private static Task<Results<Ok<IEnumerable<GameResponse>>, UnauthorizedHttpResult>> GetMyLibrary(
            ClaimsPrincipal user,
            CancellationToken cancellationToken)
        {
            string? claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(claim, out Guid uid))
            {
                return Task.FromResult<Results<Ok<IEnumerable<GameResponse>>, UnauthorizedHttpResult>>(TypedResults.Unauthorized());
            }

            // TODO: Ainda não implementado
            return Task.FromResult<Results<Ok<IEnumerable<GameResponse>>, UnauthorizedHttpResult>>(TypedResults.Ok(Enumerable.Empty<GameResponse>()));
        }

        private static Task<CreatedAtRoute> CreateGame(
            [FromBody] CreateGameRequest request,
            CancellationToken cancellationToken)
        {
            Guid newGameId = Guid.Empty;

            // TODO: Ainda não implementado
            return Task.FromResult(TypedResults.CreatedAtRoute(
                routeName: nameof(GetGameById),
                routeValues: new { id = newGameId }
            ));
        }

        private static Task<Results<Ok<GameResponse>, NotFound>> GetGameById(
            Guid id,
            CancellationToken cancellationToken)
        {
            // TODO: Ainda não implementado
            return Task.FromResult<Results<Ok<GameResponse>, NotFound>>(TypedResults.Ok(new GameResponse()));
        }
    }
}
