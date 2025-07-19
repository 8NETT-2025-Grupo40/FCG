using FCG.Domain.Common;
using FCG.Domain.Users.Entities;
using FCG.Domain.Users.ValueObjects;

namespace FCG.Domain.Users.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}