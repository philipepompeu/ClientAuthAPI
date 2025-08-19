using ClientAuthAPI.Models;

namespace ClientAuthAPI.Interfaces
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAndGenerateTokenAsync(string clientId, string clientSecret);

        Task<string?> AuthenticateUserAsync(User user, string Password);
    }
}