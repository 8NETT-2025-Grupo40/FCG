using FCG.Domain.Common;

namespace FCG.Domain.Modules.Users
{
    public interface IUserRepository : IRepository
    {
        Task<User> GetByUsernameAsync(string username);

        Task<IEnumerable<User>> GetAll();
    }
}
