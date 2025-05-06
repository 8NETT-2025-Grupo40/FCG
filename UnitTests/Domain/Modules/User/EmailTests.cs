using FCG.Domain.Common;
using FCG.Domain.Modules.Users;

namespace UnitTests.Domain.Modules.User;

public class EmailTests
{
    [Fact]
    public void CreateEmail_ShouldBeValidEmail()
    {
        Email email = new("teste@example.com");
        Assert.Equal("teste@example.com", email.Address);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateEmail_EmailIsNullOrEmpty_ShouldThrowException(string invalidEmail)
    {
        DomainException ex = Assert.Throws<DomainException>(() => new Email(invalidEmail));
        Assert.Equal("E-mail is required.", ex.Message);
    }

    [Theory]
    [InlineData("invalido")]
    [InlineData("teste@")]
    [InlineData("@exemplo.com")]
    [InlineData("teste@exemplo")]
    [InlineData("teste@.com")]
    public void CreateEmail_EmailFormatIsInvalid_ShouldThrowException(string invalidEmail)
    {
        DomainException ex = Assert.Throws<DomainException>(() => new Email(invalidEmail));
        Assert.Equal("Format of e-mail invalid.", ex.Message);
    }
}