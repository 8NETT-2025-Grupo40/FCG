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
            var result = await this._userAppService.CreateUserAsync(request, UserRole.StandardUser, cancellationToken);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            await this._userRepositoryMock.Received(1).AddAsync(Arg.Is<User>(u =>
                u.Name.ToString() == request.Name &&
                u.Email.ToString() == request.Email &&
                u.Role == UserRole.StandardUser), cancellationToken);

            await this._unitOfWorkMock.Received(1).CommitAsync(cancellationToken);
        }

        [Fact]
        public async Task CreateUserAdmin_ShouldReturnUserId()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                Name = "User Admin",
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
            var result = await this._userAppService.CreateUserAsync(request, UserRole.Admin, cancellationToken);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            await this._userRepositoryMock.Received(1).AddAsync(Arg.Is<User>(u =>
                u.Name.ToString() == request.Name &&
                u.Email.ToString() == request.Email &&
                u.Role == UserRole.Admin), cancellationToken);

            await this._unitOfWorkMock.Received(1).CommitAsync(cancellationToken);
        }

        [Fact]
        public async Task CreateUser_ButEmailExists_ShouldThrow()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                Name = "User email existing",
                Email = "existing@example.com",
                Password = "Password123@"
            };

            var cancellationToken = CancellationToken.None;

            this._userRepositoryMock
                .ExistsByEmailAsync(Arg.Is<Email>(e => e.Address == request.Email), cancellationToken)
                .Returns(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<DomainException>(() =>
                this._userAppService.CreateUserAsync(request, UserRole.StandardUser, cancellationToken));

            Assert.Equal("E-mail already in use.", ex.Message);

            await this._userRepositoryMock.Received(1)
                .ExistsByEmailAsync(Arg.Is<Email>(e => e.Address == request.Email), cancellationToken);

            await this._userRepositoryMock.DidNotReceive()
                .AddAsync(Arg.Any<User>(), cancellationToken);

            await this._unitOfWorkMock.DidNotReceive()
                .CommitAsync(cancellationToken);
        }
    }
}