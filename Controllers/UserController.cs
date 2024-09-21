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
[Route("user")]
public class UserController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<UserController> _logger;
    private readonly IGenericRepository<User> _userRepo;

    public UserController(ApplicationDbContext db, ILogger<UserController> logger, IGenericRepository<User> userRepo)
    {
        _db = db;
        _logger = logger;
        _userRepo = userRepo;
    }

    [HttpGet("list")]
    public IActionResult List()
    {
        // more of the actions on db
        // be aware of args that u put there, lambda expressions are welcomed, as: (someVar => someVar.property == columnName)
        // var obj = _db.Users.SingleOrDefault();  returns empty if no matches, object if is a match, and error if theres more matches
        // var obj = _db.Users.Single();  same as above but returns error on no matches
        // var obj = _db.Users.FirstOrDefault();  return empty if no matches, first object if theres is a match or multiples ones
        // var obj = _db.Users.First(); returns error on no matches or first row on multpiple or single match
        // var obj = _db.Users.Find();  returns the row by id (that u pass here as arg)

        // IEnumerable<User> objUserList = _db.Users;  // basic approach
        IEnumerable<User> objUserList = _userRepo.GetAll();  // general repository pattern approach
        return View(objUserList);
    }

    [HttpGet("registration")]
    public IActionResult Registration() => View();

    [HttpPost("registration")]
    [ValidateAntiForgeryToken]
    public IActionResult Registration(User obj)
    {
        _logger.LogInformation($"ModelState.IsValid: {ModelState.IsValid}");
        foreach (var modelState in ModelState)
        {
            foreach (var error in modelState.Value.Errors)
            {
                _logger.LogInformation($"ModelState Error: {error.ErrorMessage}");
            }
        }
        if(ModelState.IsValid)
        {
            // _db.Users.Add(obj);
            _userRepo.Add(obj);
            // for modification of existing object use: _db.Users.Update(obj);
            // for deleting an objecting: var obj = _db.Users.Find(id); _db.Users.Remove(obj); id is passed to the action method as argument from the url
            // _db.SaveChanges();

            // We can use a temporar storage to send just for the next request
            TempData["success"] = "Temporar message";
            return RedirectToAction("Dashboard"); // also can add the controller name as the second argument (if u want access another controller's action)
        }
        return View(obj);        
    }

    [HttpGet("login")]
    public IActionResult Login() => View();

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public IActionResult Login(User user)
    {
        // Fetch the user from the database by their email or username
        var obj = _db.Users.SingleOrDefault(u => u.Email == user.Email);
        if (obj == null)
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        if (obj.Password != user.Password)
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        // Set session data and redirect to the profile page
        HttpContext.Session.SetString("UserId", obj.Id.ToString());
        HttpContext.Session.SetString("Email", obj.Email);
        HttpContext.Session.SetString("SessionStartTime", DateTime.UtcNow.ToString("o")); // ISO 8601 format
        // return RedirectToAction("Profile");
        return RedirectToAction("Dashboard");
    }


    [HttpGet("profile")]
    public IActionResult Profile()
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

        // var user = _db.Users.SingleOrDefault(u => u.Id == userId);
        var user = _userRepo.GetById(userId);
        if (user == null)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Retrieve the session start time and check duration
        var sessionStartTimeString = HttpContext.Session.GetString("SessionStartTime");
        if (DateTime.TryParse(sessionStartTimeString, null, System.Globalization.DateTimeStyles.RoundtripKind, out var sessionStartTime))
        {
            var sessionDuration = DateTime.UtcNow - sessionStartTime;
            var timeoutDuration = TimeSpan.FromMinutes(5); // Set timeout duration

            if (sessionDuration > timeoutDuration)  // Session has expired
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            HttpContext.Session.SetString("SessionStartTime", DateTime.UtcNow.ToString("o")); // Update session start time
        }
        else
        {
            // If session start time is missing or invalid, consider session expired
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        var projects = _db.UserProjects
                        .Where(up => up.UserId == user.Id)
                        .Select(up => up.Project)
                        .ToList();

        // Pass the user and projects to the view
        var viewModel = new ProfileViewModel
        {
            User = user,
            Projects = projects
        };

        return View(viewModel);
    }

   

    [HttpGet("modify-profile")]
    public IActionResult ModifyProfile() => View();

    [HttpPost("modify-profile")]
    [ValidateAntiForgeryToken]
    public IActionResult ModifyProfile(User updatedUser)
    {
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        if (!ModelState.IsValid)
        {
            return View(updatedUser);
        }

        var user = _userRepo.GetById(userId);
        if (user == null)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password;

        _userRepo.Update(user);
        return RedirectToAction("Profile");
    }


    [HttpGet("confirm-delete-profile")]
    public IActionResult ConfirmDeleteProfile() => View();

    [HttpPost("confirm-delete-profile")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteProfile()
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

        var user = _db.Users
            .Include(u => u.UserProjects)
            .ThenInclude(up => up.Project)
            .ThenInclude(p => p.ProjectClients)
            .ThenInclude(pc => pc.Client)  // Include clients in projects
            .SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        _userRepo.Delete(user);
        HttpContext.Session.Clear(); 
        return RedirectToAction("List");
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }


    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        return RedirectToAction("Dashboard", "Project");
    }
}