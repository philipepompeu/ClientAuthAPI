using ClientAuthAPI.Models;
using MongoDB.Driver;

namespace ClientAuthAPI.Repositories;

public class ClientRepository : RepositoryBase<Client>, IClientRepository
{
    public ClientRepository(IMongoCollection<Client> collection) : base(collection)
    {
    }

    public async Task<Client?> GetClientByCredentialsAsync(string clientId, string clientSecret)
    {
        return await _collection.Find(c =>
            c.ClientId == clientId && c.ClientSecret == clientSecret).FirstOrDefaultAsync();
    }
    
}