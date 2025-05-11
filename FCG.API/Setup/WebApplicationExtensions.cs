using FCG.API.Middlewares;
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
}