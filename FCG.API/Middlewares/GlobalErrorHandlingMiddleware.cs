namespace FCG.API.Middlewares;

public class GlobalErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            const string contentType = "application/json";
            var response = context.Response;
            response.ContentType = contentType;
            response.StatusCode = 500;
            await response.WriteAsync(e.Message);
        }
    }
}