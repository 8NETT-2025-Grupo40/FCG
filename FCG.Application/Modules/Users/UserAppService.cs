using FCG.Domain.Modules.Users;

namespace FCG.Application.Modules.Users
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;


        public UserAppService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // TODO: Criar ViewModel de Usuario
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }
    }
}
