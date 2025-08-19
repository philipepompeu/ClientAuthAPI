using ClientAuthAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ClientAuthAPI.Services;

public class MongoService
{
    private readonly IMongoCollection<Client> _clients;
    private readonly IMongoCollection<User> _users;
    private readonly IMongoDatabase _database;
    public MongoService(IOptions<MongoSettings> settings)
    {
        var mongoUrl = settings.Value.ConnectionString;
        var mongoDb = settings.Value.Database;

        var client = new MongoClient(mongoUrl);
        _database = client.GetDatabase(mongoDb);

        _clients = _database.GetCollection<Client>("clients");
        _users = _database.GetCollection<User>("users");
    }


    public async Task<Client> CreateClientAsync(Client client)
    {
        await _clients.InsertOneAsync(client);
        return client;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }
    public async Task<User?> GetUserByUsernameAndClientIdAsync(string username, string clientId)
    {
        return await _users.Find(u =>
            u.Username == username && u.ClientId == clientId).FirstOrDefaultAsync();
    }

    public async Task<Client?> GetClientByCredentialsAsync(string clientId, string clientSecret)
    {
        return await _clients.Find(c =>
            c.ClientId == clientId && c.ClientSecret == clientSecret).FirstOrDefaultAsync();
    }
    
    public IMongoCollection<T> GetCollection<T>(string? collectionName = null)
    {
        var name = collectionName ?? typeof(T).Name.ToLowerInvariant() + "s";
        return _database.GetCollection<T>(name);
    }
}
