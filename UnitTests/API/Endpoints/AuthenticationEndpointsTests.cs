using FCG.API.Endpoints;
using FCG.Application.Login;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;

namespace UnitTests.API.Endpoints
{

    public class AuthenticationEndpointsTests
    {
        private readonly ILoginAppServices _loginAppServices;

        public AuthenticationEndpointsTests()
        {
            _loginAppServices = Substitute.For<ILoginAppServices>();
        }

        [Fact]
        public async Task OnLogin_WhenLoginAppAsyncReturnsSucess_ShouldReturnOkWithCorrectResult()
        {
            // Arrange
            const string sampleResponseMessage = "sampleResponseMessage";
            const string sampleToken = "sampleToken";
            const int expectedStatusCode = 200;

            var loginResult = new LoginResponse() { IsSuccess = true, Message = sampleResponseMessage, Token = sampleToken };

            _loginAppServices.LoginAppAsync(Arg.Any<LoginRequest>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(loginResult));

            var loginRequest = new LoginRequest();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await AuthenticationEndpoints.Login(_loginAppServices, loginRequest, cancellationToken);

            var resultValue = result.Result as Ok<LoginResponse>;

            // Assert
            Assert.NotNull(resultValue);
            Assert.NotNull(resultValue.Value);
            Assert.Equal(expectedStatusCode, resultValue.StatusCode);
            Assert.Equal(sampleResponseMessage, resultValue.Value.Message);
            Assert.Equal(sampleToken, resultValue.Value.Token);
        }

        [Fact]
        public async Task OnLogin_WhenLoginAppAsyncReturnsError_ShouldReturnUnauthorized()
        {
            // Arrange
            const int expectedStatusCode = 401;

            var loginResult = new LoginResponse() { IsSuccess = false };

            _loginAppServices.LoginAppAsync(Arg.Any<LoginRequest>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(loginResult));

            var loginRequest = new LoginRequest();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await AuthenticationEndpoints.Login(_loginAppServices, loginRequest, cancellationToken);

            var resultValue = result.Result as UnauthorizedHttpResult;

            // Assert
            Assert.NotNull(resultValue);
            Assert.Equal(expectedStatusCode, resultValue.StatusCode);
        }
    }
}
