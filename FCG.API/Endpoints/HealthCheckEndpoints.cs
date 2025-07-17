using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FCG.API.Endpoints
{
    public static class HealthCheckEndpoints
    {
        public static void MapHealthCheckEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapHealthChecks(
                    "/health",
                    new HealthCheckOptions
                    {
                        ResultStatusCodes =
                        {
                            [HealthStatus.Healthy] = StatusCodes.Status200OK,
                            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                            [HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable
                        },
                        ResponseWriter = async (ctx, report) =>
                        {
                            ctx.Response.ContentType = "application/json";
                            var payload = new
                            {
                                status = report.Status.ToString(),
                                checks = report.Entries.Select(e => new
                                {
                                    name = e.Key,
                                    status = e.Value.Status.ToString(),
                                    description = e.Value.Description,
                                    error = e.Value.Exception?.Message
                                })
                            };
                            await ctx.Response.WriteAsJsonAsync(payload);
                        }
                    })
                .WithName("HealthCheck");
        }
    }
}
