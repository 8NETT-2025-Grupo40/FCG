namespace FCG.Domain.Modules.Games;

public class PlayerProfileGame
{
    protected PlayerProfileGame() { }

    public PlayerProfileGame(Guid playerProfileId, Guid gameId)
    {
        this.PlayerProfileId = playerProfileId;
        this.GameId = gameId;
        this.AcquiredAt = DateTime.UtcNow;
    }

    public Guid PlayerProfileId { get; private set; }
    public PlayerProfile PlayerProfile { get; private set; } = null!;

    public Guid GameId { get; private set; }
    public Game Game { get; private set; } = null!;

    public DateTime AcquiredAt { get; private set; }
}