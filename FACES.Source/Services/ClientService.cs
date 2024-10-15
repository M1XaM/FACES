using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// For file proccessing
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;  
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using CsvHelper;

// For logs
using Microsoft.Extensions.Logging; 

using FACES.Repositories;
using FACES.RequestModels;
using FACES.ResponseModels;
using FACES.Data;
using FACES.Models;

public class ClientService : IClientService
{
    private readonly IUserRepository _userRepo;
    private readonly IProjectRepository _projectRepo;
    private readonly IClientRepository _clientRepo;
    private readonly IUserProjectRepository _userProjectRepo;
    private readonly IProjectClientRepository _projectClientRepo;
    private readonly IJwtService _jwtService;
    private readonly ILogger<ClientService> _logger;

    public ClientService(IUserRepository userRepo, IProjectRepository projectRepo, IClientRepository clientRepo, IUserProjectRepository userProjectRepo, IProjectClientRepository projectClientRepo, IJwtService jwtService, ILogger<ClientService> logger)
    {
        _userRepo = userRepo;
        _projectRepo = projectRepo;
        _clientRepo  = clientRepo;
        _userProjectRepo = userProjectRepo;
        _projectClientRepo = projectClientRepo;
        _jwtService = jwtService;
        _logger = logger;
    }

    [Authorize]
    public async Task<ClientResponse> GetClients(string projectName)
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) return new ClientResponse { Success = false, Message = "User not found." };

        var project = await _projectRepo.GetProjectByName(projectName);
        if (project == null) return new ClientResponse { Success = false,  Message = "Project with such name does not exist."};

        var userProject = await _userProjectRepo.GetProjectByUserIdAndProjectId(userId, project.Id);
        if (userProject == null) return new ClientResponse { Success = false, Message = "You do not have project with such name." };
        
        var clients = await _projectClientRepo.GetClientsByProjectId(userProject.Id);
        return new ClientResponse { Success = true, Clients = clients };
    }

    [Authorize]
    public async Task<ClientResponse> AddClient(string projectName, AddClientRequest addClientRequest)
    {

        var newClient = new Client
        {
            FirstName = addClientRequest.FirstName,
            LastName = addClientRequest.LastName,
            Email = addClientRequest.Email
        };

        await _clientRepo.AddAsync(newClient);
        return new ClientResponse { Success = true };
    }

    [Authorize]
    public async Task<ClientResponse> ImportClients(IFormFile file)
    {
        using (var reader = new StreamReader(file.OpenReadStream()))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            var records = csv.GetRecords<Client>().ToList();
            _logger.LogInformation($"Records read from CSV: {records.Count}");
            
            foreach (var record in records)
            {
                _logger.LogInformation($"Adding client: {record.FirstName} {record.LastName}");
                var client = new Client
                {
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    Email = record.Email                                                                                                                                       
                };
                var validationContext = new ValidationContext(client);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(client, validationContext, validationResults, true))
                {
                    foreach (var validationResult in validationResults)
                    {
                        _logger.LogWarning($"Validation failed for client {client.FirstName} {client.LastName}: {validationResult.ErrorMessage}");
                    }
                }
                else
                {
                    await _clientRepo.AddAsync(client);
                }
            }
            
            return new ClientResponse { Success = true };
        }
    }
}