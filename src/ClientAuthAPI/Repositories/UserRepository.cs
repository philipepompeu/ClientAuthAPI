using ClientAuthAPI.Models;
using MongoDB.Driver;

namespace ClientAuthAPI.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(IMongoCollection<User> collection) : base(collection)
    {

    }

    public async Task<User?> FindByUsernameAndClientIdAsync(string username, string clientId)
    {
        return await _collection.Find(u =>
            u.Username == username && u.ClientId == clientId).FirstOrDefaultAsync();
    }

}