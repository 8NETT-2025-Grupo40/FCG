using System.Text.Json;
using FCG.Domain.Common;

namespace FCG.API.Middlewares;

public class GlobalErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DomainException domainException)
        {
            await HandleErrorAsync(context, 422, domainException.Message);
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(context, 500, exception.Message);
        }
    }

    private static async ValueTask HandleErrorAsync(HttpContext context, int httpStatusCode, string exceptionMessage)
    {
        const string contentType = "application/json";
        var response = context.Response;
        response.ContentType = contentType;
        response.StatusCode = httpStatusCode;
        await response.WriteAsync(JsonSerializer.Serialize(exceptionMessage));
    }
}