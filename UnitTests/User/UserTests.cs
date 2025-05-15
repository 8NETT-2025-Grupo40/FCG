using FCG.Domain.Common;
using FCG.Domain.Modules.Users;

namespace UnitTests.User
{
    public class UserTests
    {
        [Fact]
        public void CreateUser_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            const BaseStatus expectedStatus = BaseStatus.Active;
            Name name = new("João");
            Email email = new("joão@example.com");
            Password password = new("SenhaValida@123");
            UserRole role = UserRole.Admin;
            var status = expectedStatus;

            // Act
            FCG.Domain.Modules.Users.User user = new(name, email, password, role, status);

            // Assert
            Assert.Equal(name, user.Name);
            Assert.Equal(email, user.Email);
            Assert.Equal(password, user.Password);
            Assert.Equal(role, user.Role);
            Assert.Equal(expectedStatus, user.Status);
            Assert.True((DateTime.Now - user.CreateDate).TotalSeconds < 1);
        }
    }
}
