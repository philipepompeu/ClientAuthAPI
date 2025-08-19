

using ClientAuthAPI.Models;

namespace ClientAuthAPI.Repositories;
public interface IUserRepository : IRepository<User>
{
    Task<User?> FindByUsernameAndClientIdAsync(string username, string clientId);
}
