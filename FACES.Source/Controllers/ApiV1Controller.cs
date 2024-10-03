using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using FACES.Repositories;
using FACES.Models;
using FACES.Data;
using FACES.RequestModels;

using Microsoft.Extensions.Logging;

using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using CsvHelper;

using System.Threading.Tasks;
using System.Text.Json;

using Microsoft.Extensions.Configuration; // For _config
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

using BCrypt.Net;

namespace FACES.Controllers;

[Route("api/v1")]
[ApiController]
public class ApiV1Controller : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ApiV1Controller> _logger;
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Project> _projectRepo;
    private readonly IGenericRepository<Client> _clientRepo;
    private readonly IEmailService _emailService;
    private readonly IJwtService _jwtService;

    public ApiV1Controller(ApplicationDbContext db, IEmailService emailService, ILogger<ApiV1Controller> logger, IGenericRepository<User> userRepo, IGenericRepository<Project> projectRepo, IGenericRepository<Client> clientRepo, IJwtService jwtService)
    {
        _db = db;
        _emailService = emailService;
        _logger = logger;
        _userRepo = userRepo;
        _projectRepo = projectRepo;
        _clientRepo = clientRepo;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
        {
            return Unauthorized(new { success = false, message = "Email and password are required.", redirectUrl = Url.Action("login") });
        }

        var obj = await _db.Users.SingleOrDefaultAsync(u => u.Email == loginRequest.Email);
        if (obj == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, obj.Password))
        {
            return Unauthorized(new { success = false, message = "Invalid email or password." });
        }
        
        var token = _jwtService.GenerateJwtToken(obj.Id.ToString());
        return Ok(new { success = true, token = token, message = "Login successful.", redirectUrl = Url.Action("ListProject", "Home", new { userId = obj.Id.ToString()})});
    }


    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody] RegistrationRequest registrationRequest)
    {
        if (string.IsNullOrEmpty(registrationRequest.FirstName) || string.IsNullOrEmpty(registrationRequest.LastName) ||
            string.IsNullOrEmpty(registrationRequest.Email) || string.IsNullOrEmpty(registrationRequest.Password))
        {
            return BadRequest(new { success = false, message = "All fields are required." });
        }

        // Check if the email already exists
        var existingUser = await _db.Users.SingleOrDefaultAsync(u => u.Email == registrationRequest.Email);
        if (existingUser != null)
        {
            return BadRequest(new { success = false, message = "Email is already in use." });
        }

        var newUser = new User
        {
            FirstName = registrationRequest.FirstName,
            LastName = registrationRequest.LastName,
            Email = registrationRequest.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registrationRequest.Password)
        };

        _userRepo.Add(newUser);
        var token = _jwtService.GenerateJwtToken(newUser.Id.ToString());
        return Ok(new { success = true, token = token, message = "Registration successful.", redirectUrl = Url.Action("ListProject", "Home", new { userId = newUser.Id})});
    }

    [HttpGet("get-user-projects")]
    [Authorize]
    public async Task<IActionResult> GetUserProjects()
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return BadRequest(new { success = false, message = "User not found." });
        }

        // Fetch the projects related to the user
        var projects = await _db.UserProjects
                                .Where(up => up.UserId == user.Id)
                                .Select(up => new 
                                {
                                    id = up.Project.Id,
                                    name = up.Project.Name,
                                    description = up.Project.Description
                                })
                                .ToListAsync();

        return Ok(new { success = true, projects = projects });
    }   

    [HttpPost("create-project")]
    [Authorize]
    public async Task<IActionResult> CreateProject([FromBody] ProjectRequest projectRequest)
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = _userRepo.GetById(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }
        if (string.IsNullOrEmpty(projectRequest.Name) || string.IsNullOrEmpty( projectRequest.Description))
        {
            return BadRequest(new { message = "Project name and description are required." });
        }

        var project = new Project
        {
            Name = projectRequest.Name,
            Description =  projectRequest.Description,
        };
        try
        {
            _projectRepo.Add(project);
            var userProject = new UserProject
            {
                UserId = user.Id,
                ProjectId = project.Id
            };
            
            _db.UserProjects.Add(userProject);
            await _db.SaveChangesAsync();
            return Ok(new 
            { 
                message = "Project created successfully.", 
                projectId = project.Id 
            });
            }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the project.", details = ex.Message });
        }
    }


    [HttpGet("project/{projectId}/get-clients")]
    [Authorize]
    public IActionResult OpenProject(int projectId)
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var project = _projectRepo.GetById(projectId);
        if (project == null)
        {
            return NotFound();
        }
        var clients = _db.ProjectClients
            .Where(pc => pc.ProjectId == projectId)
            .Select(pc => pc.Client)
            .ToList();
        return Ok(clients);
    }

    [HttpPost("project/{projectId}/add-client")]
    [Authorize]
    public async Task<IActionResult> AddClient(int projectId, [FromBody] AddClientRequest addClientRequest)
    {
        if (string.IsNullOrEmpty(addClientRequest.FirstName) || string.IsNullOrEmpty(addClientRequest.LastName))
        {
            return BadRequest(new { success = false, message = "First name and last name are required." });
        }

        var newClient = new Client
        {
            FirstName = addClientRequest.FirstName,
            LastName = addClientRequest.LastName,
        };

        _clientRepo.Add(newClient);
        return Ok();
    }


    [HttpGet("profile")]
    [Authorize]
    public IActionResult Profile()
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = _userRepo.GetById(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        var projects = _db.UserProjects
                        .Where(up => up.UserId == userId)
                        .Select(up => up.Project)
                        .ToList();

        return Ok(new { user, projects });
    }


    [HttpPost("modify-profile")]
    [Authorize]
    public IActionResult ModifyProfile([FromBody] UserRequest updatedUser)
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = _userRepo.GetById(userId);

        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password;
        _userRepo.Update(user);

        return Ok();
    }

    [HttpPost("confirm-delete-profile")]
    [Authorize]
    public IActionResult DeleteProfile()
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = _db.Users
            .Include(u => u.UserProjects)
            .ThenInclude(up => up.Project)
            .ThenInclude(p => p.ProjectClients)
            .ThenInclude(pc => pc.Client)  // Include clients in projects
            .SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return BadRequest(new { success = false, message = "User invalid."});
        }

        _userRepo.Delete(user);
        return Ok();
    }

    [HttpPost("import-clients")]
    [Authorize]
    public async Task<IActionResult> ImportClients(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            _logger.LogInformation("File is good!");
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
                        DateOfBirth = record.DateOfBirth,
                        Gender = record.Gender,
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
                        _db.Clients.Add(client);
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
                return Ok();
            }
        }

        // Handle cases where no file was provided or other errors
        ModelState.AddModelError("", "No file was uploaded or file is invalid.");
        return BadRequest();
    }

    [HttpPost("send-email")]
    [Authorize]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
    {
        IEnumerable<Client> clients = _clientRepo.GetAll();
        var emailTasks = new List<Task>();
        foreach (Client client in clients)
        {
            emailTasks.Add(_emailService.SendEmailAsync(client.Email, emailRequest.Title, emailRequest.Message));
        }

        await Task.WhenAll(emailTasks);
        return Ok();
    }
}