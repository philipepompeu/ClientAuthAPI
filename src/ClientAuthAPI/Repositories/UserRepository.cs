using ClientAuthAPI.Documents;
using ClientAuthAPI.Models;
using MongoDB.Driver;

namespace ClientAuthAPI.Repositories;

public class UserRepository : RepositoryBase<User, UserDocument>, IUserRepository
{
    public UserRepository(IMongoCollection<UserDocument> collection) : base(collection)
    {

    }

    public async Task<User?> FindByUsernameAndClientIdAsync(string username, string clientId)
    {
        var document = await _collection.Find(u =>
            u.Username == username && u.ClientId == clientId).FirstOrDefaultAsync();
        return document != null ? FromDocument(document) : null;    
    }

    public override User FromDocument(UserDocument document)
    {
        return new User
        {
            Id = document.Id,
            Username = document.Username,
            PasswordHash = document.PasswordHash,
            ClientId = document.ClientId,
            CreatedAt = document.CreatedAt
        };
    }

    protected override UserDocument ToDocument(User? entity)
    {
        return new UserDocument
        {
            Id = entity?.Id ?? string.Empty,
            Username = entity?.Username,
            PasswordHash = entity?.PasswordHash,
            ClientId = entity?.ClientId,
            CreatedAt = entity?.CreatedAt ?? DateTime.UtcNow
        };
    }
}