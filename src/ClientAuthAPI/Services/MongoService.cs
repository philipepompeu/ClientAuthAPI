using ClientAuthAPI.Documents;
using ClientAuthAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ClientAuthAPI.Services;

public class MongoService
{
    private readonly IMongoCollection<ClientDocument> _clients;
    private readonly IMongoCollection<UserDocument> _users;
    private readonly IMongoDatabase _database;
    public MongoService(IOptions<MongoSettings> settings)
    {
        var mongoUrl = settings.Value.ConnectionString;
        var mongoDb = settings.Value.Database;

        var client = new MongoClient(mongoUrl);
        _database = client.GetDatabase(mongoDb);

        _clients = _database.GetCollection<ClientDocument>("clients");
        _users = _database.GetCollection<UserDocument>("users");
    }
    
    public IMongoCollection<T> GetCollection<T>(string? collectionName = null)
    {
        var name = collectionName ?? typeof(T).Name.ToLowerInvariant() + "s";
        return _database.GetCollection<T>(name);
    }
}
