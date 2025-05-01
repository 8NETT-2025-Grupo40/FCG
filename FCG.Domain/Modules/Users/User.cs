using FCG.Domain.Modules.BaseEntities;

namespace FCG.Domain.Modules.Users
{
    public class User: BaseEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public required string Nome { get; set; }
        public string Email { get; set; }


        public bool IsValidPassword (string password){
            return PasswordHash != null && PasswordHash.Equals (password);
        }   
    }
}
