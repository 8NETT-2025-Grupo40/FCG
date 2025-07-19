using FCG.Domain.Common;
using FCG.Domain.Games.Enums;
using FCG.Domain.Games.ValueObjects;

namespace FCG.Domain.Games.Entities;

public class Game : BaseEntity
{
    protected Game() { }

    public Game(string title, string description, Genre genre, DateTime releaseDate, Price price)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Game title is required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Game description is required.");
        }

        if (genre == Genre.Unknown)
        {
            throw new DomainException("Genre must be specified.");
        }

		Title = title.Trim();
		Description = description.Trim();
		Genre = genre;
		ReleaseDate = releaseDate;
		Price = price;
		Status = BaseStatus.Active;
    }

    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public Genre Genre { get; private set; }
    public DateTime ReleaseDate { get; private set; }
    public Price Price { get; private set; } = null!;

    public List<PlayerProfileGame> PlayerProfiles { get; private set; } = new();
}