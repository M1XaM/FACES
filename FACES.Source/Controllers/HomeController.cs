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
    private readonly IJwtService _jwtService;

    public HomeController(ApplicationDbContext db, IEmailService emailService, ILogger<HomeController> logger, IGenericRepository<User> userRepo, IGenericRepository<Project> projectRepo, IGenericRepository<Client> clientRepo, IJwtService jwtService)
    {
        _db = db;
        _emailService = emailService;
        _logger = logger;
        _userRepo = userRepo;
        _projectRepo = projectRepo;
        _clientRepo = clientRepo;
        _jwtService = jwtService;
        
    }

    [HttpGet("")]
    public IActionResult Index() => View();

    [HttpGet("login")]
    public IActionResult LoginGet() => View();
    

    [HttpGet("list-project")]
    public IActionResult ListProject() => View();

    [HttpGet("project/{projectId}")]
    public IActionResult Dashboard(int projectId) => View();
    
    [HttpGet("logout")]
    public IActionResult Logout() => View();
}