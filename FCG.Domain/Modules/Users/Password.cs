using System.Security.Cryptography;
using System.Text;
using FCG.Domain.Common;

namespace FCG.Domain.Modules.Users;

public record Password
{
    private const int MinimumLength = 8;

    public string HashPassword { get; } = null!;

    public Password(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new DomainException("Password is required.");
        }

        if (!IsPasswordValid(password))
        {
            throw new DomainException("Password must have at least 8 characters, letters, numbers and special characters.");
        }

        HashPassword = ToHashPassword(password);
    }

    public static implicit operator Password(string password)
    {
        return new Password(password);
    }

    /// <summary>
    /// Realiza a criptografia da senha
    /// </summary>
    private static string ToHashPassword(string password)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool IsPasswordValid(string password)
    {
        bool hasMinimumLength = password.Length >= MinimumLength;
        bool hasDigit = password.Any(char.IsDigit);
        bool hasLetter = password.Any(char.IsLetter);
        // Pelo menos um caractere especial
        bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

        return hasMinimumLength && hasDigit && hasLetter && hasSpecialChar;
    }

    protected Password() { }
}