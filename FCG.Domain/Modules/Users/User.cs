using FCG.Domain.Common;

namespace FCG.Domain.Modules.Users;

public class User : BaseEntity
{
    protected User() { }

    public User(Name name, Email email, Password password, UserRole role)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
        CreateDate = DateTime.Now;
    }

    public Name Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public DateTime CreateDate { get; set; }
}