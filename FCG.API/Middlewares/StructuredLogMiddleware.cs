using FCG.API.Extensions;
using FCG.API.Models;

namespace FCG.API.Middlewares;

public class StructuredLogMiddleware(ILoggerFactory loggerFactory) : IMiddleware
{
    private readonly ILogger _logger = loggerFactory.CreateLogger("Logging");

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var originalResponseBody = context.Response.Body;
        
        try
        {
            context.Request.EnableBuffering();
            
            using var memoryStreamResponseBody = new MemoryStream();
            context.Response.Body = memoryStreamResponseBody;

            var log = new ApiStructuredLog(context.Request.Path);
            await next(context);
            log.FinishRequest();
            
            if (context.Response.StatusCode.IsSuccessStatusCode())
            {
                memoryStreamResponseBody.Seek(0, SeekOrigin.Begin);
                await memoryStreamResponseBody.CopyToAsync(originalResponseBody);
                log.TransformIntoSuccessfulLog(context.Response.StatusCode);
                _logger.LogInformation(log.ToString());
                return;
            }
            
            memoryStreamResponseBody.Seek(0, SeekOrigin.Begin);

            var errorMessage = await new StreamReader(memoryStreamResponseBody).ReadToEndAsync();
            
            log.TransformIntoErrorLog(errorMessage, context.Response.StatusCode);
            
            _logger.LogInformation(log.ToString());
            
            memoryStreamResponseBody.Seek(0, SeekOrigin.Begin);

            await memoryStreamResponseBody.CopyToAsync(originalResponseBody);
        }
        finally
        {
            context.Response.Body = originalResponseBody;
        }
    }
}