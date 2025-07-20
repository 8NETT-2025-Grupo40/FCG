using FCG.Domain.Common;
using FCG.Domain.Users.ValueObjects;

namespace UnitTests.Domain.Modules.User;

public class PasswordTests
{
    [Fact]
    public void CreatePassword_ValidPassword()
    {
        Password password = new("SenhaValida@123");
        Assert.False(string.IsNullOrEmpty(password.HashPassword));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreatePassword_PasswordIsNullOrEmpty_ShouldThrow(string invalidPassword)
    {
        DomainException ex = Assert.Throws<DomainException>(() => new Password(invalidPassword));
        Assert.Equal("Password is required.", ex.Message);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("abcdefgh")]
    [InlineData("12345678")]
    [InlineData("abcd1234")]
    public void CreatePassword_PasswordIsInvalid_ShouldThrow(string invalidPassword)
    {
        DomainException ex = Assert.Throws<DomainException>(() => new Password(invalidPassword));
        Assert.Equal("Password must have at least 8 characters, letters, numbers and special characters.", ex.Message);
    }
}