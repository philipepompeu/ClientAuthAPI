

using ClientAuthAPI.Application.ViewModels;
using ClientAuthAPI.Domain.Models;

namespace ClientAuthAPI.Application.Interfaces
{ 
    public interface IClientService
    {
        Task<Client> ProvisionClientAsync(ClientViewModel clientViewModel);
    }
    
}