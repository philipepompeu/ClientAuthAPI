using ClientAuthAPI.Documents;
using ClientAuthAPI.Models;
using MongoDB.Driver;

namespace ClientAuthAPI.Repositories;

public class ClientRepository : RepositoryBase<Client, ClientDocument>, IClientRepository
{
    public ClientRepository(IMongoCollection<ClientDocument> collection) : base(collection)
    {
    }


    public async Task<Client?> GetClientByCredentialsAsync(string clientId, string clientSecret)
    {
        var document = await _collection.Find(c =>
            c.ClientId == clientId && c.ClientSecret == clientSecret).FirstOrDefaultAsync();
            
        return document != null ? FromDocument(document) : null;
    }

    public override Client FromDocument(ClientDocument document)
    {
        return new Client()
        {
            Id = document.Id,
            ClientId = document.ClientId,
            ClientSecret = document.ClientSecret,
            Name = document.Name            
        };
    }
    protected override ClientDocument ToDocument(Client? entity)
    {
        return new ClientDocument()
        {
            Id = entity?.Id,
            ClientId = entity?.ClientId ?? string.Empty,
            ClientSecret = entity?.ClientSecret ?? string.Empty,
            Name = entity?.Name ?? string.Empty
        };
    }
}