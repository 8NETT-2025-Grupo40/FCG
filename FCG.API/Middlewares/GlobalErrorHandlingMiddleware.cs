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
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = 500;
            await response.WriteAsync(e.Message);
        }
    }
}