using FCG.Domain.Common;

namespace FCG.Domain.Modules.Games;

public class PlayerProfile : BaseEntity
{
    protected PlayerProfile() { }

    public PlayerProfile(Guid userId, Nickname nickname)
    {
        this.UserId = userId;
        this.Nickname = nickname;
    }

    public Guid UserId { get; private set; }
    public Nickname Nickname { get; private set; } = null!;

    private readonly List<PlayerProfileGame> _games = new();
    public IReadOnlyCollection<PlayerProfileGame> Games => this._games;

    public void AddGame(Game game)
    {
        if (this._games.Any(g => g.GameId == game.Id))
        {
            return;
        }

        this._games.Add(new PlayerProfileGame(this.Id, game.Id));
    }
}