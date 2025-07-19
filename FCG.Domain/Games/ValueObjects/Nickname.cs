using FCG.Domain.Common;

namespace FCG.Domain.Games.ValueObjects;

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
        Value = value.Trim();
    }

    public override string ToString() => Value;

    public static implicit operator Nickname(string value) => new Nickname(value);
}