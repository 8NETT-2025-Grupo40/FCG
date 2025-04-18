using FCG.Domain.Common;

namespace FCG.Domain.Modules.Users
{
    public interface IUserRepository : IRepository
    {
        Task<IEnumerable<User>> GetAll();
    }
}
