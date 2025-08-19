using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using ClientAuthAPI.Interfaces;
using ClientAuthAPI.Repositories;
using ClientAuthAPI.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace ClientAuthAPI.Services
{
    public class AuthService : IAuthService
    {        
        private readonly JwtSettings _jwt;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;



        public AuthService(IClientRepository clientRepository, IUserRepository userRepository, IOptions<JwtSettings> jwtOptions)
        {
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _jwt = jwtOptions.Value;
        }

        public async Task<string?> AuthenticateAndGenerateTokenAsync(string clientId, string clientSecret)
        {

            var client = await _clientRepository.GetClientByCredentialsAsync(clientId, clientSecret);
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

        public async Task<string?> AuthenticateUserAsync(User user, string password)
        {

            var hashPwd = PasswordHasher.HashPassword(password);

            if (user.PasswordHash != hashPwd)            
                return null;            
            
            // Cria claims (você pode adicionar mais no futuro)
            var claims = new[]
            {
                new Claim("client_id", user.ClientId),
                new Claim("username", user.Username)
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
    }
}