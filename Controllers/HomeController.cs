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

namespace FACES.Controllers;
[Route("")]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<HomeController> _logger;
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Project> _projectRepo;
    private readonly IGenericRepository<Client> _clientRepo;
    private readonly IEmailService _emailService;

    public HomeController(ApplicationDbContext db, IEmailService emailService, ILogger<HomeController> logger, IGenericRepository<User> userRepo, IGenericRepository<Project> projectRepo, IGenericRepository<Client> clientRepo)
    {
        _db = db;
        _emailService = emailService;
        _logger = logger;
        _userRepo = userRepo;
        _projectRepo = projectRepo;
        _clientRepo = clientRepo;
    }

    [HttpGet("")]
    public IActionResult Index() => View();

    [HttpGet("login")]
    public IActionResult LoginGet() => View();
    
    [HttpPost("login-post")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginPost()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        var jsonData = JObject.Parse(body);
        var email = jsonData["Email"]?.ToString();
        var password = jsonData["Password"]?.ToString();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return Json(new { success = false, message = "Email and password are required.", redirectUrl = Url.Action("login") });
        }

        var obj = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (obj == null || obj.Password != password)
        {
            return Json(new { success = false, message = "Invalid email or password." });
        }

        HttpContext.Session.SetString("UserId", obj.Id.ToString());
        HttpContext.Session.SetString("Email", obj.Email);
        HttpContext.Session.SetString("SessionStartTime", DateTime.UtcNow.ToString("o"));
        return Json(new { success = true, message = "Login successful.", redirectUrl = Url.Action("ListProject", "Home", new { userId = obj.Id.ToString()})});
    }


    [HttpPost("registration-post")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegistrationPost()
    {
        using var reader = new StreamReader(Request.Body);
        var json = await reader.ReadToEndAsync();

        _logger.LogInformation($"Received JSON: {json}");

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

        // Create a new user object
        var newUser = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password // Consider hashing this before saving
        };

        _userRepo.Add(newUser);
        await _db.SaveChangesAsync(); // Ensure changes are saved
        var newUserId = newUser.Id;

        HttpContext.Session.SetString("UserId", newUser.Id.ToString());
        HttpContext.Session.SetString("Email", newUser.Email);
        HttpContext.Session.SetString("SessionStartTime", DateTime.UtcNow.ToString("o"));
        TempData["success"] = "Registration successful";
        return Json(new { success = true, message = "Registration successful.", redirectUrl = Url.Action("ListProject", "Home", new { userId = newUserId})});
    }

    [HttpGet("user/{userId}/list-project")]
    public IActionResult ListProject(int userId)
    {
        // Check if user is authenticated
        var sessionUserId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(sessionUserId) || sessionUserId != userId.ToString())
        {
            return Json(new { success = false, message = "Unauthorized access." });
        }

        return View();
    }

    [HttpGet("user/{userId}/project/{projectId}")]
    public IActionResult Dashboard(int userId, int projectId)
    {
        return View();
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}