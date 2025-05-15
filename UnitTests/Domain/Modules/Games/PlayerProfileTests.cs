using FCG.Domain.Modules.Games;

namespace UnitTests.Domain.Modules.Games;

public class PlayerProfileTests
{
    [Fact]
    public void CreatePlayerProfile_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        Nickname nickname = new("Gamer123");

        // Act
        PlayerProfile profile = new(userId, nickname);

        // Assert
        Assert.Equal(userId, profile.UserId);
        Assert.Equal(nickname, profile.Nickname);
        Assert.NotNull(profile.Games);
        Assert.Empty(profile.Games);
    }

    [Fact]
    public void AddGame_ShouldAddOnlyOnce()
    {
        // Arrange
        var profile = new PlayerProfile(Guid.NewGuid(), new Nickname("Jogador1"));
        var game = new Game("Zelda", "jogo eletrônico no estilo hack and slash", Genre.Action, DateTime.Now.AddYears(-1), 59.99m);

        // Act
        profile.AddGame(game);
        // A segunda chamada não deve duplicar
        profile.AddGame(game);

        // Assert
        Assert.Single(profile.Games);
        var added = profile.Games.First();
        Assert.Equal(game.Id, added.GameId);
        Assert.Equal(profile.Id, added.PlayerProfileId);
    }
}