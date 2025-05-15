

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

        public async Task<LoginAppResultDTO> LoginAppAsync(LoginRequestDto requestDto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(requestDto.Email, cancellationToken);

            if (user == null || !user.CredentialsMatch(requestDto.Password))
                return LoginAppResultDTO.Fail("Invalid username or password");

            var token = _jwtTokenGenerator.GenerateToken(user);

            return LoginAppResultDTO.Success(token, "Authentication successful");
        }
    }
}