using FCG.Domain.Common;

namespace FCG.Domain.Modules.Users;

public record Name
{
    public string Value { get; }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Nome é obrigatório.");
        }

        Value = value.Trim();
    }

    public override string ToString() => Value;

    public static implicit operator Name(string name)
    {
        return new Name(name);
    }

    protected Name() { }
}