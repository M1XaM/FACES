using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

[Route("api/v1")]
public class ApiV1Controller : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<UserController> _logger;

    public ApiV1Controller(ApplicationDbContext db, ILogger<UserController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpGet("")]
    [Route("api/v1")]
    public IActionResult RedirectToMain()
    {
        return RedirectToAction(nameof(Main));
    }
 
    [HttpGet("Main")]
    public IActionResult Main()
    {
        var hardcodedData = new
        {
            Id = 1,
            Name = "John Doe",
            Email = "johndoe@example.com",
            IsActive = true
        };

        return Ok(hardcodedData);
    }

}
