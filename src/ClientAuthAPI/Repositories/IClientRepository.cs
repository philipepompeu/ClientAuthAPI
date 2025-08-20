

using ClientAuthAPI.Documents;
using ClientAuthAPI.Models;

namespace ClientAuthAPI.Repositories;

public interface IClientRepository : IRepository<Client, ClientDocument>
{
    Task<Client?> GetClientByCredentialsAsync(string clientId, string clientSecret);
}
