using System.Globalization;
using FCG.Domain.Common;

namespace FCG.Domain.Games.ValueObjects;

public record Price
{
    public decimal Value { get; }

    public Price(decimal value)
    {
        if (value < 0)
        {
            throw new DomainException("Price cannot be negative.");
        }

        Value = decimal.Round(value, 2);
    }

    public override string ToString() =>
        // Usa o formato inglês (EUA) de moeda ($1,234.56)
        Value.ToString("C2", CultureInfo.GetCultureInfo("en-US"));

    public static implicit operator Price(decimal value) => new Price(value);
}