using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using ClientAuthAPI.Interfaces;
using ClientAuthAPI.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace ClientAuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly MongoService _mongo;
        private readonly JwtSettings _jwt;

        public AuthService(MongoService mongo, IOptions<JwtSettings> jwtOptions)
        {
            _mongo = mongo;
            _jwt = jwtOptions.Value;
        }

        public async Task<string?> AuthenticateAndGenerateTokenAsync(string clientId, string clientSecret)
        {

            var client = await _mongo.GetClientByCredentialsAsync(clientId, clientSecret);
            if (client == null)
                return null;

            // Cria claims (você pode adicionar mais no futuro)
            var claims = new[]
            {
                new Claim("client_id", client.ClientId),
                new Claim("client_name", client.Name)
            };

            // Recupera configurações do JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiresInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string?> AuthenticateUserAsync(User user, string Password)
        {
            throw new NotImplementedException();
        }
    }
}