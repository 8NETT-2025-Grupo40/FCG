using FCG.Domain.Common;
using FCG.Domain.Users.ValueObjects;

namespace UnitTests.Domain.Modules.User;

public class NameTests
{
    [Fact]
    public void CreateName_ValidName()
    {
        Name name = new("NomeTeste");
        Assert.Equal("NomeTeste", name.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateName_NameIsNullOrEmpty_ShouldThrowException(string invalidName)
    {
        DomainException ex = Assert.Throws<DomainException>(() => new Name(invalidName));
        Assert.Equal("Name is required.", ex.Message);
    }
}