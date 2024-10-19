using FACES.Repositories;
using FACES.RequestModels;
using FACES.ResponseModels;
using Microsoft.AspNetCore.Http;  // For file proccessing

public interface IClientService
{
    Task<ClientServiceResponse> GetClientsAsync(string projectName);
    Task<ClientServiceResponse> AddClientAsync(string projectName, ClientViewModel addClientRequest);
    Task<ClientServiceResponse> ImportClientsAsync(IFormFile file);
    Task<ClientServiceResponse> ModifyClientAsync(string projectName, ClientViewModel updatedClient);
    Task<ClientServiceResponse> DeleteClientAsync(string email);
}