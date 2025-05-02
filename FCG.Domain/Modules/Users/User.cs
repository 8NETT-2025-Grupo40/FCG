using FCG.Domain.Modules.BaseEntities;

namespace FCG.Domain.Modules.Users
{
    public class User: BaseEntity
    {
        public User(string username, string nome, string email, string passwordHash, string status)
        {
            Username = username;
            Nome = nome;
            Email = email;
            PasswordHash = passwordHash;
            Status = status;
        }

        public string Username { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        


        //Verifica se senha está válida
        public bool IsValidPassword (string password) => PasswordHash != null && PasswordHash.Equals(password);
        
        //Verifica se usuário é ativo e senha válida, usado para authenticação
        public bool CredentialsMatch(string rawPassword) => Status.Equals("A") && IsValidPassword(rawPassword);
        
        



    }
}
