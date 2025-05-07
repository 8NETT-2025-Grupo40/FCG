using System.Text.RegularExpressions;
using FCG.Domain.Common;

namespace FCG.Domain.Modules.Users;

public record Email
{
    public string Address { get; }

    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new DomainException("E-mail é obrigatório.");
        }

        if (!IsEmailFormatValid(address))
        {
            throw new DomainException("Formato de e-mail inválido.");
        }

        Address = address;
    }

    public override string ToString() => Address;

    public static implicit operator Email(string email)
    {
        return new Email(email);
    }

    private static bool IsEmailFormatValid(string address)
    {
        // Local: não pode conter espaço nem '@'
        const string localPartPattern = @"[^@\s]+";

        // Domain: sem espaço nem '@'
        const string domainPattern = @"[^@\s]+";

        // Top Level Domain: sem espaço nem '@', após o ponto
        const string topLevelDomainPattern = @"[^@\s]+";

        return Regex.IsMatch(address, $"^{localPartPattern}@{domainPattern}\\.{topLevelDomainPattern}$", RegexOptions.IgnoreCase);
    }

    protected Email() { }
}