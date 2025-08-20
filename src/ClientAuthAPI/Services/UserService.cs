using ClientAuthAPI.Interfaces;
using ClientAuthAPI.Models;
using ClientAuthAPI.Repositories;
using ClientAuthAPI.ViewModels;
using MongoDB.Driver;

namespace ClientAuthAPI.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;    

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;    
    }

    public async Task<User> CreateUserAsync(UserViewModel request, string clientId)
    {
        var user = new User
        {
            Username = request.Username,
            PasswordHash = PasswordHasher.HashPassword(request.Password),
            ClientId = clientId,
            CreatedAt = DateTime.UtcNow,
        };

        var addedUser = await _userRepository.CreateAsync(user);

        return addedUser;
        
    }

    public async Task<User?> FindUserByNameAndClientId(string username, string clientId)
    {
        return await _userRepository.FindByUsernameAndClientIdAsync(username, clientId);
    }
}
