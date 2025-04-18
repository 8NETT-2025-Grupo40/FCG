using FCG.Domain.Modules.Users;

namespace FCG.Infrastructure.Modules.Users
{
    public class UserRepository : IUserRepository
    {
        public Task<IEnumerable<User>> GetAll()
        {
            return Task.FromResult<IEnumerable<User>>(
            [
                new User()
                {
                    Nome = "Mock1",
                },
                new User()
                {
                    Nome = "Mock2",
                }
            ]);
        }
    }
}
