using FCG.Domain.Users.Entities;

namespace FCG.Application.TokenGenerators
{
    public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
}