using FCG.Application.TokenGenerators;
using FCG.Domain.Users.Repositories;

namespace FCG.Application.Login
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

        public async Task<LoginResponse> LoginAppAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user == null || !user.CredentialsMatch(request.Password))
                return LoginResponse.Fail("Invalid email or password");

            var token = _jwtTokenGenerator.GenerateToken(user);

            return LoginResponse.Success(token, "Authentication successful");
        }
    }
}