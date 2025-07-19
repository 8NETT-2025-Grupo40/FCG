
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FCG.Application.TokenGenerators;
using FCG.Domain.Users.Entities;
using Microsoft.IdentityModel.Tokens;

namespace FCG.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtTokenGenerator(string secret, string issuer, string audience)
        {
            _secret = secret;
            _issuer = issuer;
            _audience = audience;
        }

        public string GenerateToken(User user)
        {
            string email = user.Email.ToString();
            
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Convert.FromBase64String(_secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}