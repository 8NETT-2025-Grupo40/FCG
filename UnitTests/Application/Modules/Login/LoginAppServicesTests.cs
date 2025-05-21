using FCG.Application.Modules.Login;
using FCG.Application.Modules.TokenGenerators;
using FCG.Domain.Modules.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace UnitTests.Application.Modules.Login
{
    public class LoginAppServicesTests
    {
        private readonly LoginAppServices _loginAppServices;

        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginAppServicesTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();

            _loginAppServices = new LoginAppServices(_userRepository, _jwtTokenGenerator);
        }

        [Fact]
        public async Task OnLoginAppAsync_WhenUserIsNotFound_ShouldReturnFailedResultWithCorrectValues()
        {
            // Arrange
            const string userName = "username";
            const string password = "password";
            const string expectedMessage = "Usuário ou senha inválidos";

            var cancellationToken = CancellationToken.None;

            _userRepository.GetByUsernameAsync(userName, cancellationToken).ReturnsNull();

            var loginRequest = new LoginRequestDto
            {
                Password = password,
                UserName = userName,
            };

            var expectedResult = LoginAppResultDTO.Fail(expectedMessage);

            // Act
            var result = await _loginAppServices.LoginAppAsync(loginRequest, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expectedResult, result);
        }

        [Fact]
        public async Task OnLoginAppAsync_WhenUserIsFoundAndCredentialsAreCorrect_Should()
        {
            // Arrange
            const string expectedMessage = "Usuário ou senha inválidos";
            const string sampleName = "sampleName";
            const string sampleEmail = "sample@email.com";
            const string sampleStrongPassword = "strongPassword1@";
            const string incorrectPassword = "incorrectPassword1@";

            var cancellationToken = CancellationToken.None;

            var name = new Name(sampleName);
            var email = new Email(sampleEmail);
            var password = new Password(sampleStrongPassword);

            var user = new User(name, email, password, UserRole.User, FCG.Domain.Common.BaseStatus.Active);

            _userRepository.GetByUsernameAsync(sampleName, cancellationToken).Returns(user);

            var loginRequest = new LoginRequestDto
            {
                Password = incorrectPassword,
                UserName = sampleName,
            };

            var expectedResult = LoginAppResultDTO.Fail(expectedMessage);

            // Act
            var result = await _loginAppServices.LoginAppAsync(loginRequest, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expectedResult, result);
        }

        [Fact]
        public async Task OnLoginAppAsync_WhenUserIsFoundAndCredentialsAreCorrect_ShouldReturnSucessWithCorrectToken()
        {
            // Arrange
            const string expectedMessage = "Autenticação com sucesso!";
            const string expectedToken = "token";
            const string sampleName = "sampleName";
            const string sampleEmail = "sample@email.com";
            const string sampleStrongPassword = "strongPassword1@";

            var cancellationToken = CancellationToken.None;

            var name = new Name(sampleName);
            var email = new Email(sampleEmail);
            var password = new Password(sampleStrongPassword);

            var user = new User(name, email, password, UserRole.User, FCG.Domain.Common.BaseStatus.Active);

            _userRepository.GetByUsernameAsync(sampleName, cancellationToken).Returns(user);

            var loginRequest = new LoginRequestDto
            {
                Password = sampleStrongPassword,
                UserName = sampleName,
            };

            _jwtTokenGenerator.GenerateToken(user).Returns(expectedToken);

            var expectedResult = LoginAppResultDTO.Success(expectedToken, expectedMessage);

            // Act
            var result = await _loginAppServices.LoginAppAsync(loginRequest, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(expectedResult, result);
        }
    }
}