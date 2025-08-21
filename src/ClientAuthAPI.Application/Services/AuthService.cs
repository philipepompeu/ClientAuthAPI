
using System.Security.Claims;

using System.Text;
using ClientAuthAPI.Application.Interfaces;
using ClientAuthAPI.Domain.Interfaces;
using ClientAuthAPI.Domain.Models;

namespace ClientAuthAPI.Application.Services
{
    public class AuthService : IAuthService
    {        
        
        private readonly IClientRepository _clientRepository;
        private readonly IAuthTokenService _authTokenService;
        public AuthService(IClientRepository clientRepository, IAuthTokenService authTokenService)
        {
            _clientRepository = clientRepository;            
            _authTokenService = authTokenService;
            
        }

        public async Task<string?> AuthenticateAndGenerateTokenAsync(string clientId, string clientSecret)
        {

            var client = await _clientRepository.GetClientByCredentialsAsync(clientId, clientSecret);
            if (client == null)
                return null;
                
            return _authTokenService.GenerateToken(client.ClientId, client.Name);            
        }

        public async Task<string?> AuthenticateUserAsync(User user, string password)
        {            

            if (!_authTokenService.VerifyPassword(password, user.PasswordHash))            
                return null;            
            
            return _authTokenService.GenerateToken(user);                   
        }
    }
}