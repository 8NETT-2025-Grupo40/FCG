using FCG.API.Middlewares;
using FCG.Application.Modules.Login;
using FCG.Application.Modules.Users;
using FCG.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FCG.API.Setup;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureMiddlewares(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.Use(async (context, next) =>
        {
            var token = context.Request.Headers["Authorization"].ToString();
            Console.WriteLine("Token recebido: " + token);
            await next();
        });
        app.UseAuthorization();
        app.UseMiddleware<StructuredLogMiddleware>();
        app.UseMiddleware<GlobalErrorHandlingMiddleware>();

        return app;
    }
    
    public static WebApplication ConfigureEndpoints(this WebApplication app)
    {
        app.MapGet("/games", async () => { throw new DomainException("Sample error message"); });
    
        app.MapGet("/users", async (IUserAppService userAppService) => await userAppService.GetAll())
            .WithName("")
            .WithOpenApi();

        app.MapPost("/Login", async (ILoginAppServices loginAppServices, [FromBody] LoginRequestDto request) =>
            {
                var result = await loginAppServices.LoginAppAsync(request);

                if (!result.IsSuccess)
                    return Results.Ok(result);

                return Results.Ok(result);
            })
            .WithOpenApi();

        app.MapGet("/admin", () => Results.Ok("Área restrita para Admins"))
            .WithName("GetAdminUsers")
            .WithOpenApi()
            .RequireAuthorization("AdminOnly");

        return app;
    }
}