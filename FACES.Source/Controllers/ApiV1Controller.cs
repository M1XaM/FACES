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
    private readonly IUserService _userService;
    private readonly IProjectService _projectService;
    private readonly IClientService _clientService;
    private readonly IEmailService _emailService;
    private readonly IJwtService _jwtService;
    private readonly ILogger<ApiV1Controller> _logger;

    public ApiV1Controller(IUserService userService,
                        IProjectService projectService,
                        IClientService clientService,
                        IEmailService emailService,
                        IJwtService jwtService,
                        ILogger<ApiV1Controller> logger)
    {
        _userService = userService;
        _projectService = projectService;
        _clientService = clientService;
        _emailService = emailService;
        _jwtService = jwtService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel loginRequest)
    {
        if (!ModelState.IsValid) return BadRequest(new { success = false, message = "Email and password are required." });
        var result = await _userService.LoginAsync(loginRequest);
        if (!result.Success) return Unauthorized(new { success = false, message = result.Message });
        return Ok(new { success = true, token = result.Token, message = "Login successful."});
    }


    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody] FullUserViewModel registrationRequest)
    {
        if (!ModelState.IsValid) return BadRequest(new { success = false, message = "All fields are required." });
        var result = await _userService.RegistrationAsync(registrationRequest);
        if (!result.Success) return BadRequest(new { success = false, message = result.Message});
        return Ok(new { success = true, token = result.Token, message = "Registration successful."});
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var result = await _userService.ProfileAsync();
        if (!result.Success) return BadRequest( new { success = false, message = result.Message});
        return Ok(new { user = result.User });
    }

    [HttpPut("modify-profile")]
    [Authorize]
    public async Task<IActionResult> ModifyProfile([FromBody] FullUserViewModel updatedUser)
    {
        var result = await _userService.ModifyProfileAsync(updatedUser);
        if (!result.Success) return BadRequest( new { success = false, message = result.Message});
        return Ok();
    }

    [HttpDelete("delete-profile")]
    [Authorize]
    public async Task<IActionResult> DeleteProfile()
    {
        var result = await _userService.DeleteProfileAsync();
        if (!result.Success) return BadRequest( new { success = false, message = result.Message});
        return Ok();
    }

    [HttpGet("get-user-projects")]
    [Authorize]
    public async Task<IActionResult> GetUserProjects()
    {
        var result = await _projectService.GetUserProjectsAsync();
        if (!result.Success) return BadRequest(new { success = false, message = result.Message });
        return Ok(new { success = true, projects = result.Projects });
    }   

    [HttpPost("create-project")]
    [Authorize]
    public async Task<IActionResult> CreateProject([FromBody] ProjectViewModel projectRequest)
    {
        if (!ModelState.IsValid) return BadRequest(new { message = "Project name is required." });
        var result = await _projectService.CreateProjectAsync(projectRequest);
        if (!result.Success) return BadRequest(new { success = false, message = result.Message });
        return Ok(new { succes = true });
    }


    [HttpGet("project/{projectName}/get-clients")]
    [Authorize]
    public async Task<IActionResult> GetClients(string projectName)
    {
        if (!ModelState.IsValid) return BadRequest(new { message = "Project name is required." });
        var result = await _clientService.GetClientsAsync(projectName);
        if (!result.Success) return BadRequest(new { success = false, message = result.Message });
        return Ok(new { succes = true, clients = result.Clients});
    }

    [HttpPost("project/{projectName}/add-client")]
    [Authorize]
    public async Task<IActionResult> AddClient(string projectName, [FromBody] ClientViewModel newClient)
    {
        if (!ModelState.IsValid) return BadRequest(new { success = false, message = "First name, last name or email are not valid." });
        var result = await _clientService.AddClientAsync(projectName, newClient);
        if (!result.Success) return BadRequest(new { success = false, message = result.Message });
        return Ok();
    }

    [HttpPut("project/{projectName}/modify-client/{clientId}")]
    [Authorize]
    public async Task<IActionResult> ModifyClient(string projectName, [FromBody] ClientViewModel updatedClient)
    {
        if (!ModelState.IsValid) return BadRequest(new { success = false, message = "First name, last name or email are not valid." });
        var result = await _clientService.ModifyClientAsync(projectName, updatedClient);
        if (!result.Success) return BadRequest(new { success = false, message = result.Message });
        return Ok();
    }

    [HttpPost("project/{projectName}/import-clients")]
    [Authorize]
    public async Task<IActionResult> ImportClients(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var result = await _clientService.ImportClientsAsync(file);
            if (!result.Success) return BadRequest(new { success = false, message = result.Message });
            return Ok();
        }
        // Handle cases where no file was provided or other errors
        ModelState.AddModelError("", "No file was uploaded or file is invalid.");
        return BadRequest();
    }

    [HttpPost("send-email")]
    [Authorize]
    public async Task<IActionResult> SendEmail([FromBody] EmailViewModel emailRequest)
    {
        if (!ModelState.IsValid) return BadRequest(new { success = false, message = "Title and the message are required." });
        var result = await _emailService.SendEmailAsync(emailRequest);
        if (!result.Success) return BadRequest(new { success = false, message = result.Message });
        return Ok();
    }

    [HttpPost("verify-token")]
    public async Task<IActionResult> TokenVerification()
    {
        var result = await _jwtService.TokenVerificationAsync();
        if (!result) return BadRequest(new { valid = false, message = "Token verification failed." });
        return Ok(new { valid = true });
    }
}