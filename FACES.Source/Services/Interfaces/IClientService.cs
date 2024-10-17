using FACES.Repositories;
using FACES.RequestModels;
using FACES.ResponseModels;
using Microsoft.AspNetCore.Http;  // For file proccessing

public interface IClientService
{
    Task<ClientResponse> GetClients(string projectName);
    Task<ClientResponse> AddClient(string projectName, AddClientRequest addClientRequest);
    Task<ClientResponse> ImportClients(IFormFile file);
    // Task<ClientResponse> ModifyClient();
    // Task<ClientResponse> DeleteClient();
}