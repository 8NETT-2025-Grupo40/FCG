using FCG.API.Endpoints;
using FCG.Application.Modules.Login;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;

namespace UnitTests.Endpoints
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

            var loginResult = new LoginAppResultDTO() { IsSuccess = true, Message = sampleResponseMessage, Token = sampleToken };

            _loginAppServices.LoginAppAsync(Arg.Any<LoginRequestDto>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(loginResult));

            var loginRequestDto = new LoginRequestDto();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await AuthenticationEndpoints.Login(_loginAppServices, loginRequestDto, cancellationToken);

            var resultValue = result.Result as Ok<LoginAppResultDTO>;

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

            var loginResult = new LoginAppResultDTO() { IsSuccess = false };

            _loginAppServices.LoginAppAsync(Arg.Any<LoginRequestDto>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(loginResult));

            var loginRequestDto = new LoginRequestDto();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await AuthenticationEndpoints.Login(_loginAppServices, loginRequestDto, cancellationToken);

            var resultValue = result.Result as UnauthorizedHttpResult;

            // Assert
            Assert.NotNull(resultValue);
            Assert.Equal(expectedStatusCode, resultValue.StatusCode);
        }
    }
}
