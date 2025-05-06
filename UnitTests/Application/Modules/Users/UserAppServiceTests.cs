using FCG.Application.Modules.Users;
using FCG.Domain.Common;
using FCG.Domain.Modules.Users;
using NSubstitute;

namespace UnitTests.Application.Modules.Users;

public class UserAppServiceTests
{
    public class CreateUserTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IUserRepository _userRepositoryMock;
        private readonly UserAppService _userAppService;

        public CreateUserTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _unitOfWorkMock.UserRepository.Returns(_userRepositoryMock);
            _userAppService = new UserAppService(_unitOfWorkMock);
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

            _userRepositoryMock
                .AddAsync(
                    Arg.Is<User>(u => u.Name == request.Name && u.Email == request.Email),
                    cancellationToken).Returns(Task.CompletedTask);
            _unitOfWorkMock.CommitAsync(cancellationToken).Returns(Task.FromResult(1));

            // Act
            var result = await _userAppService.CreateUserAsync(request, UserRole.StandardUser, cancellationToken);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            await _userRepositoryMock.Received(1).AddAsync(Arg.Is<User>(u =>
                u.Name.ToString() == request.Name &&
                u.Email.ToString() == request.Email &&
                u.Role == UserRole.StandardUser), cancellationToken);

            await _unitOfWorkMock.Received(1).CommitAsync(cancellationToken);
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

            _userRepositoryMock
                .AddAsync(
                    Arg.Is<User>(u => u.Name == request.Name && u.Email == request.Email),
                    cancellationToken).Returns(Task.CompletedTask);
            _unitOfWorkMock.CommitAsync(cancellationToken).Returns(Task.FromResult(1));

            // Act
            var result = await _userAppService.CreateUserAsync(request, UserRole.Admin, cancellationToken);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            await _userRepositoryMock.Received(1).AddAsync(Arg.Is<User>(u =>
                u.Name.ToString() == request.Name &&
                u.Email.ToString() == request.Email &&
                u.Role == UserRole.Admin), cancellationToken);

            await _unitOfWorkMock.Received(1).CommitAsync(cancellationToken);
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

            _userRepositoryMock
                .ExistsByEmailAsync(Arg.Is<Email>(e => e.Address == request.Email), cancellationToken)
                .Returns(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<DomainException>(() =>
                _userAppService.CreateUserAsync(request, UserRole.StandardUser, cancellationToken));

            Assert.Equal("E-mail already in use.", ex.Message);

            await _userRepositoryMock.Received(1)
                .ExistsByEmailAsync(Arg.Is<Email>(e => e.Address == request.Email), cancellationToken);

            await _userRepositoryMock.DidNotReceive()
                .AddAsync(Arg.Any<User>(), cancellationToken);

            await _unitOfWorkMock.DidNotReceive()
                .CommitAsync(cancellationToken);
        }
    }
}