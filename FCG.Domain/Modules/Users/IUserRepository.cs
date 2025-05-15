using FCG.Domain.Common;

namespace FCG.Domain.Modules.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
    }
}