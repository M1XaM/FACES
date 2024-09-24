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

namespace FACES.Controllers;

[Route("api/v1")]
public class ApiV1Controller : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<UserController> _logger;
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Project> _projectRepo;
    private readonly IGenericRepository<Client> _clientRepo;
    private readonly IEmailService _emailService;

    public ApiV1Controller(ApplicationDbContext db, IEmailService emailService, ILogger<UserController> logger, IGenericRepository<User> userRepo, IGenericRepository<Project> projectRepo, IGenericRepository<Client> clientRepo)
    {
        _db = db;
        _emailService = emailService;
        _logger = logger;
        _userRepo = userRepo;
        _projectRepo = projectRepo;
        _clientRepo = clientRepo;
    }

    [HttpGet("user/{userId}/get-list-project")]
    public async Task<IActionResult> GetUserProjects(int userId)
    {
        // Fetch the user
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

        // Return the list of projects as JSON
        return Json(new 
        { 
            success = true, 
            projects = projects 
        });
    }   

    [HttpPost("user/{userId}/create-project")]
    public async Task<IActionResult> CreateProject(int userId)
    {
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized(new { message = "User not authenticated." });
        }
        if (!int.TryParse(userIdString, out var sessionUserId) || sessionUserId != userId)
        {
            HttpContext.Session.Clear();
            return Unauthorized(new { message = "Invalid user session." });
        }
        var user = _userRepo.GetById(userId);
        if (user == null)
        {
            HttpContext.Session.Clear();
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


    [HttpGet("user/{userId}/project/{projectId}/get-clients")]
    public IActionResult OpenProject(int userId, int projectId)
    {
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

    [HttpGet("user/{userId}/project/{projectId}/add-clients")]
    public IActionResult AddClient(int userId, int projectId)
    {
        var obj = new AddClientViewModel
        {
            Project = _projectRepo.GetById(projectId),
            Client = new Client()
        };
        return View(obj);
    }

}