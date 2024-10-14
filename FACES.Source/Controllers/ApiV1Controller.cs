using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using FACES.Repositories;
using FACES.Data;
using FACES.Models;
using FACES.RequestModels;
using FACES.ResponseModels;


namespace FACES.Controllers;
[Route("api/v1")]
[ApiController]
public class ApiV1Controller : ControllerBase
{
    private readonly ILogger<ApiV1Controller> _logger;
    private readonly IUserService _userService;
    private readonly IProjectService _projectService;
    private readonly IClientService _clientService;
    private readonly IEmailService _emailService;

    public ApiV1Controller(IUserService userService,  IProjectService projectService, IClientService clientService, IEmailService emailService, ILogger<ApiV1Controller> logger)
    {
        _userService = userService;
        _projectService = projectService;
        _clientService = clientService;
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid) return BadRequest(new { success = false, message = "Email and password are required." });
        var result = await _userService.Login(loginRequest);
        if (!result.Success) return Unauthorized(new { success = false, message = result.Message });
        return Ok(new { success = true, token = result.Token, message = "Login successful."});
    }


    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody] FullUserRequest registrationRequest)
    {
        if (!ModelState.IsValid) return BadRequest(new { success = false, message = "All fields are required." });
        var result = await _userService.Registration(registrationRequest);
        if (!result.Success) return BadRequest(new { success = false, message = result.Message});
        return Ok(new { success = true, token = result.Token, message = "Registration successful."});
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var result = await _userService.Profile();
        if (!result.Success) return BadRequest( new { success = false, message = result.Message});
        return Ok(new { user = result.User });
    }

    [HttpPost("modify-profile")]
    [Authorize]
    public async Task<IActionResult> ModifyProfile([FromBody] FullUserRequest updatedUser)
    {
        var result = await _userService.ModifyProfile(updatedUser);
        if (!result.Success) return BadRequest( new { success = false, message = result.Message});
        return Ok();
    }

    [HttpPost("delete-profile")]
    [Authorize]
    public async Task<IActionResult> DeleteProfile()
    {
        var result = await _userService.DeleteProfile();
        if (!result.Success) return BadRequest( new { success = false, message = result.Message});
        return Ok();
    }

    [HttpGet("get-user-projects")]
    [Authorize]
    public async Task<IActionResult> GetUserProjects()
    {
        var result = await _projectService.GetUserProjects();
        if (!result.Success) return BadRequest(new { success = false, message = result.Message });
        return Ok(new { success = true, projects = result.Projects });
    }   

    [HttpPost("create-project")]
    [Authorize]
    public async Task<IActionResult> CreateProject([FromBody] ProjectRequest projectRequest)
    {
        if (!ModelState.IsValid) return BadRequest(new { message = "Project name is required." });
        var result = await _projectService.CreateProject(projectRequest);
        if (!result.Success) return BadRequest(new { success = false, message = result.Message });
        return Ok(new { succes = true });
    }


    [HttpGet("project/{projectName}/get-clients")]
    [Authorize]
    public async Task<IActionResult> GetClients(string projectName)
    {
        if (!ModelState.IsValid) return BadRequest(new { message = "Project name is required." });
        var result = await _clientService.GetClients(projectName);
        if (!result.Success) return BadRequest(new { success = false, message = result.Message });
        return Ok(new { succes = true, clients = result.Clients});
    }

    [HttpPost("project/{projectName}/add-client")]
    [Authorize]
    public async Task<IActionResult> AddClient(string projectName, [FromBody] AddClientRequest addClientRequest)
    {
        if (!ModelState.IsValid) return BadRequest(new { success = false, message = "First name, last name or email are not valid." });
        var result = await _clientService.AddClient(projectName, addClientRequest);
        if (!result.Success) return BadRequest(new { success = false, message = result.Message });
        return Ok();
    }

    [HttpPost("project/{projectName}/import-clients")]
    [Authorize]
    public async Task<IActionResult> ImportClients(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var result = await _clientService.ImportClients(file);
            if (!result.Success) return BadRequest(new { success = false, message = result.Message });
            return Ok();
        }
        // Handle cases where no file was provided or other errors
        ModelState.AddModelError("", "No file was uploaded or file is invalid.");
        return BadRequest();
    }

    [HttpPost("send-email")]
    [Authorize]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
    {
        if (!ModelState.IsValid) return BadRequest(new { success = false, message = "Title and the message are required." });
        var result = await _emailService.SendEmail(emailRequest);
        if (!result.Success) return BadRequest(new { success = false, message = result.Message });
        return Ok();
    }
}