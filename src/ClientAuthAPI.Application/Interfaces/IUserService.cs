


using ClientAuthAPI.Application.ViewModels;
using ClientAuthAPI.Domain.Models;

namespace ClientAuthAPI.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserViewModel request, string clientId);
        Task<User?> FindUserByNameAndClientId(string username, string clientId);
    }
    
}