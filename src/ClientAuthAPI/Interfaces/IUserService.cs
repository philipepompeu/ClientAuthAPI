using ClientAuthAPI.Models;
using ClientAuthAPI.ViewModels;

namespace ClientAuthAPI.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserViewModel request, string clientId);
        Task<User?> FindUserByNameAndClientId(string username, string clientId);
    }
    
}