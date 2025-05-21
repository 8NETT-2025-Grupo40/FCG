using System.Text;
using FCG.API.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UnitTests.Extensions;

namespace UnitTests.API.Middlewares;

public class StructuredLogMiddlewareTests
{
    private readonly StructuredLogMiddleware _structuredLogMiddleware;
    private readonly ILogger<StructuredLogMiddleware> _loggerMock;

    public StructuredLogMiddlewareTests()
    {
        _loggerMock = Substitute.For<ILogger<StructuredLogMiddleware>>();
        
        _structuredLogMiddleware = new StructuredLogMiddleware(_loggerMock);
    }

    [Fact]
    public async Task OnMiddlewareExecution_WhenResponseIsSuccessful_ShouldLogInformationLogContainingSuccess()
    {
        // Arrange
        const int successfulStatusCode = 200;
        const string expectedStatusCode = "200";
        const string expectedInformationMessage = "Success";
        
        var requestDelegate = new RequestDelegate(_ => Task.CompletedTask);
        
        var context = new DefaultHttpContext
        {
            Response =
            {
                StatusCode = successfulStatusCode
            }
        };

        // Act
        await _structuredLogMiddleware.InvokeAsync(context, requestDelegate);
        
        // Assert
        _loggerMock.Verify(LogLevel.Information, 1, [expectedInformationMessage, expectedStatusCode]);
    }
    
    [Fact]
    public async Task OnMiddlewareExecution_WhenResponseIsNotSuccessful_Should()
    {
        // Arrange
        const int unsuccessfulStatusCode = 500;
        const string expectedErrorMessage = "Sample Error Message";
        const string expectedMessage = "Please check ErrorMessage property for more details.";
        const string expectedStatusCode = "500";
        
        var context = new DefaultHttpContext();

        var memoryStreamResponseBody = new MemoryStream();
        var messageInBytes = Encoding.UTF8.GetBytes(expectedErrorMessage);
        
        await memoryStreamResponseBody.WriteAsync(messageInBytes);
        
        // Act
        await _structuredLogMiddleware.InvokeAsync(context, RequestDelegate);
        
        // Assert
        _loggerMock.Verify(LogLevel.Error, 1, [expectedErrorMessage, expectedMessage, expectedStatusCode]);
        return;

        async Task RequestDelegate(HttpContext ctx)
        {
            context.Response.StatusCode = unsuccessfulStatusCode;
            await ctx.Response.WriteAsync(expectedErrorMessage);
        }
    }
}