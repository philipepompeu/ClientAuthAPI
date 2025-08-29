using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClientAuthAPI.Application.Interfaces;
using ClientAuthAPI.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ClientAuthAPI.Infrastructure.Security.Services
{

    internal sealed class AuthTokenService : IAuthTokenService
    {
        private readonly JwtSettings _jwt;
        public AuthTokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwt = jwtOptions.Value;
        }
        public string GenerateToken(User user)
        {
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

        public string GenerateToken(string clientId, string clientName)
        {            
            var claims = new[]
            {
                new Claim("client_id", clientId),
                new Claim("client_name", clientName)
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

        public string HashPassword(string password)
        {
            return PasswordHasher.HashPassword(password);
        }

        public bool VerifyPassword(string plainText, string passwordHash)
        {
            return PasswordHasher.HashPassword(plainText) == passwordHash;
        }
    }

}

