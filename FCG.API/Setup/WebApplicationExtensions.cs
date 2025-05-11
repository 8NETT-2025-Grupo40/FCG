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
        // Declara primeiro o Middeware que captura erros da pipeline. Pois se der erro na pipeline, ele deveria proteger
        app.UseMiddleware<GlobalErrorHandlingMiddleware>();
        app.UseMiddleware<StructuredLogMiddleware>();

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseSwaggerConfiguration();

        app.UseAuthentication();
        app.UseAuthorization();

        app.Use(async (context, next) =>
        {
            var token = context.Request.Headers["Authorization"].ToString();
            Console.WriteLine("Token recebido: " + token);
            await next();
        });

        return app;
    }

    public static WebApplication ConfigureEndpoints(this WebApplication app)
    {
        // TODO: na branch da issue #17, inserir as classes de endpoints aqui
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