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

using Microsoft.Extensions.Logging;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using CsvHelper;

using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration; // For _config
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

using BCrypt.Net;

namespace FACES.Controllers;

[Route("api/v1")]
public class ApiV1Controller : Controller
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

    [HttpPost("login-post")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginPost()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        var jsonData = JObject.Parse(body);
        var email = jsonData["Email"].ToString();
        var password = jsonData["Password"].ToString();
        

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return Json(new { success = false, message = "Email and password are required.", redirectUrl = Url.Action("login") });
        }

        var obj = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (obj == null || !BCrypt.Net.BCrypt.Verify(password, obj.Password))
        {
            return Json(new { success = false, message = "Invalid email or password." });
        }
        
        var token = _jwtService.GenerateJwtToken(obj.Id.ToString());
        return Json(new { success = true, token = token, message = "Login successful.", redirectUrl = Url.Action("ListProject", "Home", new { userId = obj.Id.ToString()})});
    }


    [HttpPost("registration-post")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegistrationPost()
    {
        using var reader = new StreamReader(Request.Body);
        var json = await reader.ReadToEndAsync();

        // Deserialize the JSON into a dynamic object or a specific model
        var jsonData = JObject.Parse(json);

        var firstName = jsonData["FirstName"]?.ToString();
        var lastName = jsonData["LastName"]?.ToString();
        var email = jsonData["Email"]?.ToString();
        var password = jsonData["Password"]?.ToString();

        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
            string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return Json(new { success = false, message = "All fields are required." });
        }

        // Check if the email already exists
        var existingUser = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (existingUser != null)
        {
            return Json(new { success = false, message = "Email is already in use." });
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        // Create a new user object
        var newUser = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = hashedPassword // Consider hashing this before saving
        };

        _userRepo.Add(newUser);
        await _db.SaveChangesAsync(); // Ensure changes are saved
        var newUserId = newUser.Id;

        var token = _jwtService.GenerateJwtToken(newUserId.ToString());
        return Json(new { success = true, token = token, message = "Registration successful.", redirectUrl = Url.Action("ListProject", "Home", new { userId = newUserId})});
    }

    [HttpGet("get-list-project")]
    [Authorize]
    public async Task<IActionResult> GetUserProjects()
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return Json(new { success = false, message = "User not found." });
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

        return Json(new 
        { 
            success = true, 
            projects = projects 
        });
    }   

    [HttpPost("create-project")]
    [Authorize]
    public async Task<IActionResult> CreateProject()
    {
        int userId = _jwtService.ExtractUserIdFromToken();
        var user = _userRepo.GetById(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        var jsonData = JObject.Parse(body);
        var name = jsonData["name"]?.ToString();
        var description = jsonData["description"]?.ToString();

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
        {
            return BadRequest(new { message = "Project name and description are required." });
        }

        var project = new Project
        {
            Name = name,
            Description = description,
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
            return new JsonResult(new 
            { 
                message = "Project created successfully.", 
                projectId = project.Id 
            })
            {
                StatusCode = 200
            };
            }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the project.", 
            details = ex.Message });
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
        return Json(clients);
    }

    // [HttpGet("user/{userId}/project/{projectId}/add-clients")]
    // public IActionResult AddClient(int userId, int projectId)
    // {
    //     var obj = new AddClientViewModel
    //     {
    //         Project = _projectRepo.GetById(projectId),
    //         Client = new Client()
    //     };
    //     return View(obj);
    // }

    // For future implementation
    // [HttpGet("profile")]
    // public IActionResult Profile()
    // {
    //     var userIdString = HttpContext.Session.GetString("UserId");
    //     if (string.IsNullOrEmpty(userIdString))
    //     {
    //         return RedirectToAction("Login");
    //     }

    //     if (!int.TryParse(userIdString, out var userId))
    //     {
    //         HttpContext.Session.Clear();
    //         return RedirectToAction("Login");
    //     }

    //     // var user = _db.Users.SingleOrDefault(u => u.Id == userId);
    //     var user = _userRepo.GetById(userId);
    //     if (user == null)
    //     {
    //         HttpContext.Session.Clear();
    //         return RedirectToAction("Login");
    //     }

    //     // Retrieve the session start time and check duration
    //     var sessionStartTimeString = HttpContext.Session.GetString("SessionStartTime");
    //     if (DateTime.TryParse(sessionStartTimeString, null, System.Globalization.DateTimeStyles.RoundtripKind, out var sessionStartTime))
    //     {
    //         var sessionDuration = DateTime.UtcNow - sessionStartTime;
    //         var timeoutDuration = TimeSpan.FromMinutes(5); // Set timeout duration

    //         if (sessionDuration > timeoutDuration)  // Session has expired
    //         {
    //             HttpContext.Session.Clear();
    //             return RedirectToAction("Login");
    //         }

    //         HttpContext.Session.SetString("SessionStartTime", DateTime.UtcNow.ToString("o")); // Update session start time
    //     }
    //     else
    //     {
    //         // If session start time is missing or invalid, consider session expired
    //         HttpContext.Session.Clear();
    //         return RedirectToAction("Login");
    //     }

    //     var projects = _db.UserProjects
    //                     .Where(up => up.UserId == user.Id)
    //                     .Select(up => up.Project)
    //                     .ToList();

    //     // Pass the user and projects to the view
    //     var viewModel = new ProfileViewModel
    //     {
    //         User = user,
    //         Projects = projects
    //     };

    //     return View(viewModel);
    // }

    // [HttpPost("modify-profile")]
    // [ValidateAntiForgeryToken]
    // public IActionResult ModifyProfile(User updatedUser)
    // {
    //     var userIdString = HttpContext.Session.GetString("UserId");
    //     if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
    //     {
    //         HttpContext.Session.Clear();
    //         return RedirectToAction("Login");
    //     }
    //     if (!ModelState.IsValid)
    //     {
    //         return View(updatedUser);
    //     }

    //     var user = _userRepo.GetById(userId);
    //     if (user == null)
    //     {
    //         HttpContext.Session.Clear();
    //         return RedirectToAction("Login");
    //     }

    //     user.FirstName = updatedUser.FirstName;
    //     user.LastName = updatedUser.LastName;
    //     user.Email = updatedUser.Email;
    //     user.Password = updatedUser.Password;

    //     _userRepo.Update(user);
    //     return RedirectToAction("Profile");
    // }

    // [HttpPost("confirm-delete-profile")]
    // [ValidateAntiForgeryToken]
    // public IActionResult DeleteProfile()
    // {
    //     var userIdString = HttpContext.Session.GetString("UserId");
    //     if (string.IsNullOrEmpty(userIdString))
    //     {
    //         return RedirectToAction("Login");
    //     }

    //     if (!int.TryParse(userIdString, out var userId))
    //     {
    //         HttpContext.Session.Clear();
    //         return RedirectToAction("Login");
    //     }

    //     var user = _db.Users
    //         .Include(u => u.UserProjects)
    //         .ThenInclude(up => up.Project)
    //         .ThenInclude(p => p.ProjectClients)
    //         .ThenInclude(pc => pc.Client)  // Include clients in projects
    //         .SingleOrDefault(u => u.Id == userId);

    //     if (user == null)
    //     {
    //         HttpContext.Session.Clear();
    //         return RedirectToAction("Login");
    //     }

    //     _userRepo.Delete(user);
    //     HttpContext.Session.Clear(); 
    //     return RedirectToAction("List");
    // }

    // [HttpPost("import-clients")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> ImportClients(IFormFile file)
    // {
    //     if (file != null && file.Length > 0)
    //     {
    //         _logger.LogInformation("File is good!");
    //         using (var reader = new StreamReader(file.OpenReadStream()))
    //         using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
    //         {
    //             var records = csv.GetRecords<ClientImportModel>().ToList();
    //             _logger.LogInformation($"Records read from CSV: {records.Count}");
                
    //             foreach (var record in records)
    //             {
    //                 _logger.LogInformation($"Adding client: {record.FirstName} {record.LastName}");
    //                 var client = new Client
    //                 {
    //                     FirstName = record.FirstName,
    //                     LastName = record.LastName,
    //                     DateOfBirth = record.DateOfBirth,
    //                     Gender = record.Gender,
    //                 };

    //                 var validationContext = new ValidationContext(client);
    //                 var validationResults = new List<ValidationResult>();

    //                 if (!Validator.TryValidateObject(client, validationContext, validationResults, true))
    //                 {
    //                     foreach (var validationResult in validationResults)
    //                     {
    //                         _logger.LogWarning($"Validation failed for client {client.FirstName} {client.LastName}: {validationResult.ErrorMessage}");
    //                     }
    //                 }
    //                 else
    //                 {
    //                     _db.Clients.Add(client);
    //                 }
    //             }
                
    //             try
    //             {
    //                 await _db.SaveChangesAsync();
    //                 _logger.LogInformation("Is saved for user");
    //             }
    //             catch (Exception ex)
    //             {
    //                 _logger.LogError($"Error saving changes to database: {ex.Message}");
    //             }
    //             return RedirectToAction("Profile");
    //         }
    //     }

    //     // Handle cases where no file was provided or other errors
    //     ModelState.AddModelError("", "No file was uploaded or file is invalid.");
    //     return View("ImportClients");
    // }

    // public async Task<IActionResult> SendEmail(int id)
    // {
    //     IEnumerable<Client> clients = _clientRepo.GetAll();
    //     var emailTasks = new List<Task>();
    //     foreach (Client client in clients)
    //     {
    //         emailTasks.Add(_emailService.SendEmailAsync(client.Email, "Welcome!", "Hello, this is a test email."));
    //     }

    //     await Task.WhenAll(emailTasks);
    //     return RedirectToAction("OpenProject", new { id = id});
    // }


}