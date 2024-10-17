using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FACES.Controllers;
[Route("")]
public class HomeController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();

    [HttpGet("login")]
    public IActionResult Login() => View();
    

    [HttpGet("list-project")]
    public IActionResult ListProject() => View();

    [HttpGet("project/{projectId}")]
    public IActionResult Dashboard(int projectId) => View();
    
    [HttpGet("logout")]
    public IActionResult Logout() => View();
}