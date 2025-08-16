using ClientAuthAPI.Interfaces;
using ClientAuthAPI.Models;
using ClientAuthAPI.ViewModels;
using System.Security.Cryptography;

namespace ClientAuthAPI.Services;

public class ClientService:IClientService
{
    private readonly MongoService _mongo;

    public ClientService(MongoService mongo)
    {
        _mongo = mongo;
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

        await _mongo.CreateClientAsync(client);

        return client;
    }
}
