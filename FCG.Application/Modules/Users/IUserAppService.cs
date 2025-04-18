using FCG.Domain.Modules.Users;

namespace FCG.Application.Modules.Users;

public interface IUserAppService
{
    Task<IEnumerable<User>> GetAll();
}