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
            return Task.FromResult<IEnumerable<User>>(
            [
                new User("NomeMock1", "mock@outlook.com", "Mock@1234", UserRole.Admin)
            ]);
        }
    }
}
