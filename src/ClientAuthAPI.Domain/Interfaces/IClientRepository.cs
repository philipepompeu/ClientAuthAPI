

using ClientAuthAPI.Domain.Models;

namespace ClientAuthAPI.Domain.Interfaces;

public interface IClientRepository : IRepository<Client>
{
    Task<Client?> GetClientByCredentialsAsync(string clientId, string clientSecret);
}
