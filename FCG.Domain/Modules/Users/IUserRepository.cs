using FCG.Domain.Common;

namespace FCG.Domain.Modules.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetAll();
    }
}
