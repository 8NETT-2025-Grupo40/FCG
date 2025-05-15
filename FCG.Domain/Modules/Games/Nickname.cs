using FCG.Domain.Common;

namespace FCG.Domain.Modules.Games;

public record Nickname
{
    public string Value { get; } = null!;
    protected Nickname() { }

    public Nickname(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Nickname is required.");
        }

        if (value.Length is < 3 or > 20)
        {
            throw new DomainException("Nickname must be between 3 and 20 characters long.");
        }

        // Validação de caracteres pode ser incluída
        this.Value = value.Trim();
    }

    public override string ToString() => this.Value;

    public static implicit operator Nickname(string value) => new Nickname(value);
}