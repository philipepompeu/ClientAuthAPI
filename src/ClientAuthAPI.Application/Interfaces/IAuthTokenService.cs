using ClientAuthAPI.Domain.Models;

namespace ClientAuthAPI.Application.Interfaces
{
    public interface IAuthTokenService
    {
        string GenerateToken(User user);
        string GenerateToken(string clientId, string clientName);
        bool VerifyPassword(string plainText, string passwordHash);
        string HashPassword(string password);
    }

}