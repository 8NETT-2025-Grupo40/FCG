using FCG.Domain.Modules.Games;

namespace FCG.Application.Modules.Games
{
    public class CreateGameRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Genre Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
    }
}
