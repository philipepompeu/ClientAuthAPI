

using ClientAuthAPI.Documents;
using ClientAuthAPI.Models;

namespace ClientAuthAPI.Repositories;
public interface IUserRepository : IRepository<User, UserDocument>
{
    Task<User?> FindByUsernameAndClientIdAsync(string username, string clientId);
}
