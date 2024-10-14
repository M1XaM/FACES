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
    private readonly ApplicationDbContext _db;
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Project> _projectRepo;
    private readonly IGenericRepository<Client> _clientRepo;
    private readonly ILogger<ClientService> _logger;
    private readonly IJwtService _jwtService;

    public ClientService(IGenericRepository<Client> clientRepo, ILogger<ClientService> logger, ApplicationDbContext db, IGenericRepository<User> userRepo, IJwtService jwtService, IGenericRepository<Project> projectRepo)
    {
        _db = db;
        _userRepo = userRepo;
        _jwtService = jwtService;
        _projectRepo = projectRepo;
        _clientRepo  = clientRepo;
        _logger = logger;
    }

    [Authorize]
    public async Task<ClientResponse> GetClients(string projectName)
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null) return new ClientResponse { Success = false, Message = "User not found." };

        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Name == projectName);
        if (project == null) return new ClientResponse { Success = false,  Message = "Project with such name does not exist."};

        var userProject = await _db.UserProjects.FirstOrDefaultAsync(up => up.UserId == userId && up.ProjectId == project.Id);
        if (userProject == null) return new ClientResponse { Success = false, Message = "You do not have project with such name." };
        if (project == null) return new ClientResponse { Success = false, Message = "Project with such name does not exist."};
        
        var clients = await _db.ProjectClients
            .Where(pc => pc.ProjectId == project.Id)
            .Select(pc => pc.Client)
            .ToListAsync();
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
                    await _db.Clients.AddAsync(client);
                }
            }
            
            try
            {
                await _db.SaveChangesAsync();
                _logger.LogInformation("Is saved for user");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving changes to database: {ex.Message}");
            }
            return new ClientResponse { Success = true };
        }
    }
}