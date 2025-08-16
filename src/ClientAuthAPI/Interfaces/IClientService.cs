using ClientAuthAPI.Models;
using ClientAuthAPI.ViewModels;

namespace ClientAuthAPI.Interfaces
{ 
    public interface IClientService
    {
        Task<Client> ProvisionClientAsync(ClientViewModel request);
    }
    
}