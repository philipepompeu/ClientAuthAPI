

using ClientAuthAPI.Application.Interfaces;
using ClientAuthAPI.Application.ViewModels;
using ClientAuthAPI.Domain.Interfaces;
using ClientAuthAPI.Domain.Models;

namespace ClientAuthAPI.Application.Services;

internal sealed class UserService: IUserService
{
    private readonly IUserRepository _userRepository;    
    private readonly IAuthTokenService _authTokenService;
    public UserService(IUserRepository userRepository, IAuthTokenService authTokenService)
    {
        _userRepository = userRepository;    
        _authTokenService = authTokenService;
    }

    public async Task<User> CreateUserAsync(UserViewModel request, string clientId)
    {
        var user = new User
        {
            Username = request.Username,
            PasswordHash = _authTokenService.HashPassword(request.Password),            
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
