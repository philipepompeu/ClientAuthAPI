using ClientAuthAPI.Interfaces;
using ClientAuthAPI.Models;
using ClientAuthAPI.Repositories;
using ClientAuthAPI.ViewModels;
using System.Security.Cryptography;

namespace ClientAuthAPI.Services;

public class ClientService:IClientService
{    
    private readonly IRepository<Client> _clientRepository;

    public ClientService(IRepository<Client> repository)
    {
        _clientRepository = repository;
    }

    public async Task<Client> ProvisionClientAsync(ClientViewModel request)
    {
        var clientId = Guid.NewGuid().ToString("N");

        var secretBytes = RandomNumberGenerator.GetBytes(32);
        var clientSecret = Convert.ToBase64String(secretBytes);

        var client = new Client
        {
            Name = request.Name,
            ClientId = clientId,
            ClientSecret = clientSecret
        };

        await _clientRepository.CreateAsync(client);

        return client;
    }
}
