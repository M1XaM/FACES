using FACES.Repositories;
using FACES.RequestModels;
using FACES.ResponseModels;
using Microsoft.AspNetCore.Http;  // For file proccessing

public interface IClientService
{
    Task<ClientServiceResponse> GetClientsAsync(string projectName);
    Task<ClientServiceResponse> AddClientAsync(string projectName, ClientViewRequest addClientRequest);
    Task<ClientServiceResponse> ImportClientsAsync(IFormFile file);
    Task<ClientServiceResponse> ModifyClientAsync(string projectName, ClientViewRequest updatedClient);
    Task<ClientServiceResponse> DeleteClientAsync(string email);
}