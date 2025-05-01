using FCG.Domain.Common;
using FCG.Domain.Modules.Users;

namespace UnitTests.User;

public class NameTests
{
    [Fact]
    public void CreateName_ValidName()
    {
        Name name = new("Anderson");
        Assert.Equal("Anderson", name.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateName_NameIsNullOrEmpty_ShouldThrowException(string invalidName)
    {
        DomainException ex = Assert.Throws<DomainException>(() => new Name(invalidName));
        Assert.Equal("Nome é obrigatório.", ex.Message);
    }
}