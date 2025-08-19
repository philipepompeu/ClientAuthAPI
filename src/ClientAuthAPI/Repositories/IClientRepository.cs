

using ClientAuthAPI.Models;

namespace ClientAuthAPI.Repositories;

public interface IClientRepository : IRepository<Client>
{
    Task<Client?> GetClientByCredentialsAsync(string clientId, string clientSecret);
}
