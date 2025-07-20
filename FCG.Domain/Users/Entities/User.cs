using FCG.Domain.Common;
using FCG.Domain.Users.Enums;
using FCG.Domain.Users.ValueObjects;

namespace FCG.Domain.Users.Entities;

public class User : BaseEntity
{
    protected User() { }

    public User(Name name, Email email, Password password, UserRole role, BaseStatus status)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
        Status = status;
    }

    public Name Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public UserRole Role { get; private set; }

    /// <summary>
    /// Verifica se usuário é ativo e senha válida, usado para authenticação
    /// </summary>
    public bool CredentialsMatch(string rawPassword) => Status.Equals(BaseStatus.Active) && IsValidPassword(rawPassword);

    /// <summary>
    /// Verifica se senha está válida
    /// </summary>
    private bool IsValidPassword (string password) => Password.Equals(password);

}