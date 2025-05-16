using FCG.Domain.Common;
using FCG.Domain.Modules.Users;

namespace UnitTests.Users;

public class UserTests
{
    [Fact]
    public void CreateUser_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        Name name = new("João");
        Email email = new("joão@example.com");
        Password password = new("SenhaValida@123");
        UserRole role = UserRole.Admin;

        // Act
        User user = new(name, email, password, role, BaseStatus.Active);

        // Assert
        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.Password);
        Assert.Equal(role, user.Role);
    }
}