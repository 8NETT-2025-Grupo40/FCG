using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace UnitTests.Extensions;

public static class LoggerExtensions
{
    public static void Verify(this ILogger logger, LogLevel logLevel, int numberOfCalls, string expectedMessage)
    {
        logger.Received(numberOfCalls).Log(
            logLevel,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString().Contains(expectedMessage, StringComparison.OrdinalIgnoreCase)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>()
        );
    }
    
    public static void Verify(this ILogger logger, LogLevel logLevel, int numberOfCalls, string[] expectedValues)
    {
        logger.Received(numberOfCalls).Log(
            logLevel,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => VerifyMatches(o.ToString(), expectedValues)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>()
        );
    }

    private static bool VerifyMatches(string value, string[] expectedValues) => 
        expectedValues.All(expectedValue => value.Contains(expectedValue, StringComparison.OrdinalIgnoreCase));

    public static void VerifyItWasNeverCalled(this ILogger logger)
    {
        logger.DidNotReceiveWithAnyArgs().Log(
            Arg.Any<LogLevel>(),
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>()
        );
    }
}