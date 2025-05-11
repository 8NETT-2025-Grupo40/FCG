using FCG.Domain.Common;
using System.Text.Json;

namespace FCG.API.Middlewares;

public class GlobalErrorHandlingMiddleware(ILogger logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DomainException domainException)
        {
            logger.LogError("There was an error while processing the request: {DomainExceptionMessage}", domainException.Message);
            await HandleErrorAsync(context, 422, domainException.Message);
        }
        catch (Exception exception)
        {
            logger.LogError("There was an error while processing the request: {ExceptionMessage}", exception.Message);
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