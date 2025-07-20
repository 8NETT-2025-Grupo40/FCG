using FCG.Domain.Common;
using FCG.Domain.Users.Enums;
using FCG.Domain.Users.ValueObjects;

namespace UnitTests.Domain.Modules.User
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
            FCG.Domain.Users.Entities.User user = new(name, email, password, role, status);

            // Assert
            Assert.Equal(name, user.Name);
            Assert.Equal(email, user.Email);
            Assert.Equal(password, user.Password);
            Assert.Equal(role, user.Role);
            Assert.Equal(expectedStatus, user.Status);
        }
    }
}
