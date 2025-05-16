using FCG.Domain.Common;

namespace FCG.Domain.Modules.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}