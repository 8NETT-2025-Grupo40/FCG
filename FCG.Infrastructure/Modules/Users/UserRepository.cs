using FCG.Domain.Common;
using FCG.Domain.Modules.Users;


namespace FCG.Infrastructure.Modules.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _appContext;

        public UserRepository(ApplicationDbContext appContext)
        {
            _appContext = appContext;
        }

        public Task<IEnumerable<User>> GetAll()
        {
            return Task.FromResult<IEnumerable<User>>(
            [
                new User("NomeMock1", "mock@outlook.com", "Mock@1234", UserRole.Admin, BaseStatus.Active)
            ]);
        }

        public Task<User> GetByUsernameAsync(string username)
        {
            return Task.FromResult(new User("NomeMock1", "mock@outlook.com", "Mock@1234", UserRole.Admin, BaseStatus.Active));
        }
    }
}
