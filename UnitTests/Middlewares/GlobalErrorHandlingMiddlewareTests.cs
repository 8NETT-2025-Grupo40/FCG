using System.Text.Json;
using FCG.API.Middlewares;
using FCG.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UnitTests.Extensions;

namespace UnitTests.Middlewares;

public class GlobalErrorHandlingMiddlewareTests
{
    private readonly GlobalErrorHandlingMiddleware _globalErrorHandlingMiddleware;
    private readonly ILogger<GlobalErrorHandlingMiddleware> _loggerMock;

    public GlobalErrorHandlingMiddlewareTests()
    {
        _loggerMock = Substitute.For<ILogger<GlobalErrorHandlingMiddleware>>();

        _globalErrorHandlingMiddleware = new GlobalErrorHandlingMiddleware(_loggerMock);
    }

    [Fact]
    public async Task OnMiddlewareExecution_WhenNoExceptionsAreThrown_ShouldNeverCallLogMethods()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var requestDelegate = new RequestDelegate(_ => Task.CompletedTask);

        // Act
        await _globalErrorHandlingMiddleware.InvokeAsync(context, requestDelegate);

        // Assert
        _loggerMock.VerifyItWasNeverCalled();
    }

    [Fact]
    public async Task OnMiddlewareExecution_WhenDomainExceptionIsThrown_ShouldReturnCorrectContextAndLogCorrectErrorMessage()
    {
        // Arrange
        const int expectedNumberOfCalls = 1;
        const int expectedHttpStatusCode = 422;

        const string exceptionMessage = "exceptionMessage";
        const string expectedErrorLogMessage = $"There was an error while processing the request: {exceptionMessage}";
        const string applicationJson = "application/json";

        var expectedResponseMessage = JsonSerializer.Serialize(exceptionMessage);

        var requestDelegate = new RequestDelegate(_ => throw new DomainException(exceptionMessage));

        var context = new DefaultHttpContext();
        var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        // Act
        await _globalErrorHandlingMiddleware.InvokeAsync(context, requestDelegate);

        // Assert
        memoryStream.Seek(0, SeekOrigin.Begin);
        var errorMessage = await new StreamReader(context.Response.Body).ReadToEndAsync();

        Assert.Equal(expectedResponseMessage, errorMessage);
        Assert.Equal(expectedHttpStatusCode, context.Response.StatusCode);
        Assert.Equal(applicationJson, context.Response.ContentType);

        _loggerMock.Verify(LogLevel.Error, expectedNumberOfCalls, expectedErrorLogMessage);
    }

    [Fact]
    public async Task OnMiddlewareExecution_WhenCommonExceptionIsThrown_ShouldReturnCorrectContextAndLogCorrectErrorMessage()
    {
        // Arrange
        const int expectedNumberOfCalls = 1;
        const int expectedHttpStatusCode = 500;

        const string exceptionMessage = "exceptionMessage";
        const string expectedErrorLogMessage = $"There was an error while processing the request: {exceptionMessage}";
        const string applicationJson = "application/json";

        var expectedResponseMessage = JsonSerializer.Serialize(exceptionMessage);

        var requestDelegate = new RequestDelegate(_ => throw new Exception(exceptionMessage));

        var context = new DefaultHttpContext();
        var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        // Act
        await _globalErrorHandlingMiddleware.InvokeAsync(context, requestDelegate);

        // Assert
        memoryStream.Seek(0, SeekOrigin.Begin);
        var errorMessage = await new StreamReader(context.Response.Body).ReadToEndAsync();

        Assert.Equal(expectedResponseMessage, errorMessage);
        Assert.Equal(expectedHttpStatusCode, context.Response.StatusCode);
        Assert.Equal(applicationJson, context.Response.ContentType);

        _loggerMock.Verify(LogLevel.Critical, expectedNumberOfCalls, expectedErrorLogMessage);
    }
}