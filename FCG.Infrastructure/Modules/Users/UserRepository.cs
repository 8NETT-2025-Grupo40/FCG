using FCG.Domain.Modules.Users;
using FCG.Infrastructure.Common;

namespace FCG.Infrastructure.Modules.Users
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
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
