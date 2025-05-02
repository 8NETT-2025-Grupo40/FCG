using FCG.Domain.Modules.Users;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Modules.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly DbAppContext _appContext;

        public UserRepository(DbAppContext appContext)
        {
            _appContext = appContext;
        }

        public Task<IEnumerable<User>> GetAll()
        {
                var users = new List<User>
                {
                    new User
                    (
                        "user@fcg.com.br",
                        "User1234",  
                        "Mock2",
                        "user@fcg.com.br",
                        "A"
                    ),
                    new User
                    (
                        "user@fcg.com.br",
                        "User1234",  
                        "Mock2",
                        "user@fcg.com.br",                        
                        "A"
                    )

                };

                return Task.FromResult<IEnumerable<User>>(users);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await Task.FromResult(new User("adm@fcg.com.br", "Administrador", "adm@fcg.com.br", "Adm1234", "A"));
        }

    }
}
