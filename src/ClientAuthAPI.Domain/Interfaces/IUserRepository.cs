



using ClientAuthAPI.Domain.Models;

namespace ClientAuthAPI.Domain.Interfaces;
public interface IUserRepository : IRepository<User>
{
    Task<User?> FindByUsernameAndClientIdAsync(string username, string clientId);
}
