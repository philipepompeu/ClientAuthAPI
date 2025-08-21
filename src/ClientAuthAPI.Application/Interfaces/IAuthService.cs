using ClientAuthAPI.Domain.Models;

namespace ClientAuthAPI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAndGenerateTokenAsync(string clientId, string clientSecret);

        Task<string?> AuthenticateUserAsync(User user, string password);
    }
}