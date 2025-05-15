using FCG.Domain.Common;

namespace FCG.Domain.Modules.Users;

public class User : BaseEntity
{
    protected User() { }

    public User(Name name, Email email, Password password, UserRole role, BaseStatus status)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
        CreateDate = DateTime.Now;
        Status = status;
    }

    public Name Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public DateTime CreateDate { get; set; }

    //Verifica se senha está válida, TODO: Acrescentar regra de Senha
    public bool IsValidPassword (string password) => Password != null && Password.Equals(password);
    
    //Verifica se usuário é ativo e senha válida, usado para authenticação
    public bool CredentialsMatch(string rawPassword) => Status.Equals(BaseStatus.Active) && IsValidPassword(rawPassword);
    
    
}