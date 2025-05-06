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
            const string email = "user@fcg.com";
            const string password = "password";
            const string expectedMessage = "Invalid username or password";

            var cancellationToken = CancellationToken.None;

            _userRepository.GetByEmailAsync(email, cancellationToken).ReturnsNull();

            var loginRequest = new LoginRequestDto
            {
                Password = password,
                Email = email,
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
            const string expectedMessage = "Invalid username or password";
            const string sampleName = "sampleName";
            const string sampleEmail = "sample@email.com";
            const string sampleStrongPassword = "strongPassword1@";
            const string incorrectPassword = "incorrectPassword1@";

            var cancellationToken = CancellationToken.None;

            var name = new Name(sampleName);
            var email = new Email(sampleEmail);
            var password = new Password(sampleStrongPassword);

            var user = new User(name, email, password, UserRole.StandardUser, FCG.Domain.Common.BaseStatus.Active);

            _userRepository.GetByEmailAsync(sampleName, cancellationToken).Returns(user);

            var loginRequest = new LoginRequestDto
            {
                Password = incorrectPassword,
                Email = sampleEmail,
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
            const string expectedMessage = "Authentication successful";
            const string expectedToken = "token";
            const string sampleName = "sampleName";
            const string sampleEmail = "sample@email.com";
            const string sampleStrongPassword = "strongPassword1@";

            var cancellationToken = CancellationToken.None;

            var name = new Name(sampleName);
            var email = new Email(sampleEmail);
            var password = new Password(sampleStrongPassword);

            var user = new User(name, email, password, UserRole.StandardUser, FCG.Domain.Common.BaseStatus.Active);

            _userRepository.GetByEmailAsync(sampleEmail, cancellationToken).Returns(user);

            var loginRequest = new LoginRequestDto
            {
                Password = sampleStrongPassword,
                Email = sampleEmail,
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