using FCG.Domain.Modules.Users;

namespace FCG.Application.Modules.TokenGenerators
{
    public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
}