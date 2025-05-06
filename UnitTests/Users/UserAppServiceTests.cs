using FCG.Application.Modules.Users;
using FCG.Domain.Common;
using FCG.Domain.Modules.Users;
using NSubstitute;

namespace UnitTests.Users;

public class UserAppServiceTests
{
    public class CreateUserTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IUserRepository _userRepositoryMock;
        private readonly UserAppService _userAppService;

        public CreateUserTests()
        {
            this._unitOfWorkMock = Substitute.For<IUnitOfWork>();
            this._userRepositoryMock = Substitute.For<IUserRepository>();
            this._unitOfWorkMock.UserRepository.Returns(this._userRepositoryMock);
            this._userAppService = new UserAppService(this._unitOfWorkMock);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnUserId()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "Password123@"
            };

            var cancellationToken = CancellationToken.None;

            this._userRepositoryMock
                .AddAsync(
                    Arg.Is<User>(u => u.Name == request.Name && u.Email == request.Email),
                    cancellationToken).Returns(Task.CompletedTask);
            this._unitOfWorkMock.CommitAsync(cancellationToken).Returns(Task.FromResult(1));

            // Act
            var result = await this._userAppService.CreateUserAsync(request, cancellationToken);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            await this._userRepositoryMock.Received(1).AddAsync(Arg.Is<User>(u =>
                u.Name.ToString() == request.Name &&
                u.Email.ToString() == request.Email &&
                u.Role == UserRole.User), cancellationToken);

            await this._unitOfWorkMock.Received(1).CommitAsync(cancellationToken);
        }
    }
}