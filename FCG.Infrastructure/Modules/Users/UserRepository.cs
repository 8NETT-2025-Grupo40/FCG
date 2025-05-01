using FCG.Domain.Modules.Users;

namespace FCG.Infrastructure.Modules.Users
{
    public class UserRepository : IUserRepository
    {
        public Task<IEnumerable<User>> GetAll()
        {
            return Task.FromResult<IEnumerable<User>>(
            [
                new User("NomeMock1", "mock@outlook.com", "Mock@1234", UserRole.Admin)
            ]);
        }
    }
}
