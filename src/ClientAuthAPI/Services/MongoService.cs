using ClientAuthAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ClientAuthAPI.Services;

public class MongoService
{
    private readonly IMongoCollection<Client> _clients;
    public MongoService(IOptions<MongoSettings> settings)
    {
        var mongoUrl = settings.Value.ConnectionString;
        var mongoDb = settings.Value.Database;        

        var client = new MongoClient(mongoUrl);
        var database = client.GetDatabase(mongoDb);

        _clients = database.GetCollection<Client>("clients");
    }

    public async Task<Client> CreateClientAsync(Client client)
    {
        await _clients.InsertOneAsync(client);
        return client;
    }

    public async Task<Client?> GetClientByCredentialsAsync(string clientId, string clientSecret)
    {
        return await _clients.Find(c =>
            c.ClientId == clientId && c.ClientSecret == clientSecret).FirstOrDefaultAsync();
    }
}
