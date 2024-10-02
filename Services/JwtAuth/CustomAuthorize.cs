using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration; // For _config
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging; // For _logging
using System.Security.Claims;
using System.Diagnostics;
using System.Text;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute, IAuthorizationFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CustomAuthorizeAttribute> _logger;

    public CustomAuthorizeAttribute(IHttpContextAccessor httpContextAccessor, ILogger<CustomAuthorizeAttribute> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        
        // Check for token in the Authorization header
        var token = context.HttpContext.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        var config = _httpContextAccessor.HttpContext.RequestServices.GetService<IConfiguration>();
        var key = Encoding.ASCII.GetBytes(config["Jwt:Key"]);
        
        if (string.IsNullOrEmpty(token) || !ValidateToken(token, key))
        {
            context.Result = new UnauthorizedResult();
        }

    }

    private bool ValidateToken(string token, byte[] key)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // Set to true if you want to validate the issuer
                ValidateAudience = false, // Set to true if you want to validate the audience
                ClockSkew = TimeSpan.Zero // Prevent token expiration issues
            }, out SecurityToken validatedToken);

            // If the token is valid, return true
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Token validation failed: {ex.Message}");
            return false; // Token is not valid
        }
    }
}