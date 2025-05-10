

using FCG.Application.Modules.TokenGenerators;
using FCG.Domain.Modules.Users;

namespace FCG.Application.Modules.Login
{
    public class LoginAppServices : ILoginAppServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginAppServices(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginAppResultDTO> LoginAppAsync(LoginRequestDto requestDto)
        {
            var user = await _userRepository.GetByUsernameAsync(requestDto.UserName);

            if (user == null || !user.CredentialsMatch(requestDto.Password))
                return LoginAppResultDTO.Fail("Usuário ou senha inválidos");

            var token = _jwtTokenGenerator.GenerateToken(user);

            return LoginAppResultDTO.Success(token, "Autenticação com sucesso!");
        }
    }
}