
using System.Security.Cryptography;
using ClientAuthAPI.Application.Interfaces;
using ClientAuthAPI.Application.ViewModels;
using ClientAuthAPI.Domain.Interfaces;
using ClientAuthAPI.Domain.Models;

namespace ClientAuthAPI.Application.Services;

internal sealed class ClientService:IClientService
{    
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository repository)
    {
        _clientRepository = repository;
    }

    public async Task<Client> ProvisionClientAsync(ClientViewModel clientViewModel)
    {
        var clientId = Guid.NewGuid().ToString("N");

        var secretBytes = RandomNumberGenerator.GetBytes(32);
        var clientSecret = Convert.ToBase64String(secretBytes);

        var client = new Client
        {
            Name = clientViewModel.Name,
            ClientId = clientId,
            ClientSecret = clientSecret
        };

        await _clientRepository.CreateAsync(client);

        return client;
    }
}
