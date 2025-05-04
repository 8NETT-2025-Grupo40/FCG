using FCG.API.Middlewares;

namespace FCG.API.Extensions;

public static class MiddlewareExtensions
{
    public static void ConfigureMiddlewares(this IApplicationBuilder app) => 
        app.UseMiddleware<StructuredLogMiddleware>().UseMiddleware<GlobalErrorHandlingMiddleware>();
}