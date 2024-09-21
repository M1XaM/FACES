using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FACES.Repositories;
using FACES.Models;
using FACES.Data;


using Microsoft.Extensions.Logging;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using CsvHelper;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace FACES.Controllers;
[Route("project")]
public class ProjectController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<UserController> _logger;
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Project> _projectRepo;
    private readonly IGenericRepository<Client> _clientRepo;
    private readonly IEmailService _emailService;

    public ProjectController(ApplicationDbContext db, IEmailService emailService, ILogger<UserController> logger, IGenericRepository<User> userRepo, IGenericRepository<Project> projectRepo, IGenericRepository<Client> clientRepo)
    {
        _db = db;
        _emailService = emailService;
        _logger = logger;
        _userRepo = userRepo;
        _projectRepo = projectRepo;
        _clientRepo = clientRepo;
    }

    [HttpGet("create-project")]
    public IActionResult GetCreateProject() => View();

    [HttpPost("create-project")]
    [ValidateAntiForgeryToken]
    public IActionResult CreateProject(Project newProject)
    {

        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString))
        {
            return RedirectToAction("Login");
        }

        if (!int.TryParse(userIdString, out var userId))
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        var user = _userRepo.GetById(userId);
        if (user == null)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        _logger.LogInformation($"ModelState.IsValid: {ModelState.IsValid}");
        foreach (var modelState in ModelState)
        {
            foreach (var error in modelState.Value.Errors)
            {
                _logger.LogInformation($"ModelState Error: {error.ErrorMessage}");
            }
        }

        if (ModelState.IsValid)
        {
            var project = new Project
            {
                Name = newProject.Name,
            };
            _projectRepo.Add(project);

            var userProject = new UserProject
            {
                UserId = user.Id,
                ProjectId = project.Id
            };
            _db.UserProjects.Add(userProject);
            _db.SaveChanges();

            return RedirectToAction("OpenProject", new { id = userProject.ProjectId}); // Redirect to the project list or another appropriate view
        }

        return View("GetCreateProject");
    }

    [HttpGet("open-project")]
    public IActionResult OpenProject(int id)
    {
        var project = _projectRepo.GetById(id);
        if (project == null)
        {
            return NotFound();
        }

        var clients = _db.ProjectClients
            .Where(pc => pc.ProjectId == id)
            .Select(pc => pc.Client)
            .ToList();

        var viewModel = new ProjectViewModel
        {
            Project = project,
            Clients = clients
        };
        return View("ProjectList", viewModel);
    }


    [HttpGet("add-client")]
    public IActionResult AddClient(int projectId)
    {
        var obj = new AddClientViewModel
        {
            Project = _projectRepo.GetById(projectId),
            Client = new Client()
        };
        return View(obj);
    }

    [HttpPost("add-client")]
    [ValidateAntiForgeryToken]
    public IActionResult AddClient(AddClientViewModel viewModel, int projectId)
    {
        _logger.LogInformation($"ModelState.IsValid: {ModelState.IsValid}");
        foreach (var modelState in ModelState)
        {
            foreach (var error in modelState.Value.Errors)
            {
                _logger.LogInformation($"ModelState Error: {error.ErrorMessage}");
            }
        }

        var project = _projectRepo.GetById(projectId);
        viewModel.Project = project;

        if (ModelState.IsValid)
        {
            _clientRepo.Add(viewModel.Client);
            var projectClient = new ProjectClient
            {
                ProjectId = projectId,
                ClientId = viewModel.Client.Id
            };

            _db.ProjectClients.Add(projectClient);
            _db.SaveChanges();

            return RedirectToAction("OpenProject", new { id = projectId }); // Redirect to the project page
        }

        // If model state is invalid, redisplay the form with existing data
        return View("AddClient", viewModel);
    }

    [HttpGet("import-clients")]
    public IActionResult ImportClients() => View();
    
    [HttpPost("import-clients")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportClients(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            _logger.LogInformation("File is good!");
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = csv.GetRecords<ClientImportModel>().ToList();
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
                return RedirectToAction("Profile");
            }
        }

        // Handle cases where no file was provided or other errors
        ModelState.AddModelError("", "No file was uploaded or file is invalid.");
        return View("ImportClients");
    }

    public async Task<IActionResult> SendEmail(int id)
    {
        IEnumerable<Client> clients = _clientRepo.GetAll();
        var emailTasks = new List<Task>();
        foreach (Client client in clients)
        {
            emailTasks.Add(_emailService.SendEmailAsync(client.Email, "Welcome!", "Hello, this is a test email."));
        }

        await Task.WhenAll(emailTasks);
        return RedirectToAction("OpenProject", new { id = id});
    }

    [HttpGet("dashboard")]
    public IActionResult Dashboard() => View();
}