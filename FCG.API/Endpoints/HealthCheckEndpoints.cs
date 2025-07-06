using FCG.Infrastructure;

namespace FCG.API.Endpoints
{
    public static class HealthCheckEndpoints
    {
        public static void MapHealthCheckEndpoints(this IEndpointRouteBuilder app)
        {
            // Agrupa todos os endpoints de health sob o prefixo "/health"
            RouteGroupBuilder group = app
                .MapGroup("/health")
                .WithTags("Health");

            group.MapGet("/", Health)
                .WithName("HealthCheck")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status503ServiceUnavailable);
        }

        // Handler interno: verifica a conexão com o banco
        private static async Task<IResult> Health(
            ApplicationDbContext db,
            CancellationToken cancellationToken)
        {
            bool canConnect = await db.Database
                .CanConnectAsync(cancellationToken);

            if (!canConnect)
            {
                return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            return Results.Ok(new { status = "Healthy" });
        }
    }
}
