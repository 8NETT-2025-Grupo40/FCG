using FCG.Domain.Common;
using FCG.Domain.Games.ValueObjects;

namespace FCG.Domain.Games.Entities;

public class PlayerProfile : BaseEntity
{
    protected PlayerProfile() { }

    public PlayerProfile(Guid userId, Nickname nickname)
    {
		UserId = userId;
		Nickname = nickname;
    }

    public Guid UserId { get; private set; }
    public Nickname Nickname { get; private set; } = null!;

    private readonly List<PlayerProfileGame> _games = new();
    public IReadOnlyCollection<PlayerProfileGame> Games => _games;

    public void AddGame(Game game)
    {
        if (_games.Any(g => g.GameId == game.Id))
        {
            return;
        }

		_games.Add(new PlayerProfileGame(Id, game.Id));
    }
}