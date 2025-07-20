using FCG.Domain.Common;
using FCG.Domain.Games.Entities;
using FCG.Domain.Games.Enums;
using FCG.Domain.Games.ValueObjects;

namespace UnitTests.Domain.Modules.Games;

public class GameTests
{
    [Fact]
    public void CreateGame_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        string title = "Joguinho maneiro";
        string description = "Um jogo massa";
        Genre genre = Genre.Action;
        DateTime releaseDate = new DateTime(2024, 10, 15);
        Price price = new Price(59.99m);

        // Act
        var game = new Game(title, description, genre, releaseDate, price);

        // Assert
        Assert.Equal(title, game.Title);
        Assert.Equal(description, game.Description);
        Assert.Equal(genre, game.Genre);
        Assert.Equal(releaseDate, game.ReleaseDate);
        Assert.Equal(price, game.Price);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void CreateGame_ButInvalidTitle_ShouldThrow(string invalidTitle)
    {
        var ex = Assert.Throws<DomainException>(() =>
            new Game(invalidTitle, "desc", Genre.Action, DateTime.Today, new Price(10))
        );
        Assert.Equal("Game title is required.", ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void CreateGame_ButInvalidDescription_ShouldThrow(string invalidDescription)
    {
        var ex = Assert.Throws<DomainException>(() =>
            new Game("Valid title", invalidDescription, Genre.Action, DateTime.Today, new Price(10))
        );
        Assert.Equal("Game description is required.", ex.Message);
    }

    [Fact]
    public void CreateGame_ButUnknownGenre_ShouldThrow()
    {
        var ex = Assert.Throws<DomainException>(() =>
            new Game("Title", "Desc", Genre.Unknown, DateTime.Today, new Price(10))
        );
        Assert.Equal("Genre must be specified.", ex.Message);
    }

    [Fact]
    public void CreateGame_ButNegativePrice_ShouldThrow()
    {
        var ex = Assert.Throws<DomainException>(() =>
            new Game("Title", "Desc", Genre.Action, DateTime.Today, new Price(-5))
        );
        Assert.Equal("Price cannot be negative.", ex.Message);
    }
}