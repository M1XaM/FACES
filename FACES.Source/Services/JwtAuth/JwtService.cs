using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _config = config;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateJwtToken(string userId)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("X1eRvn=qbaxlC#3IJt8W-4k82N70Gsddv5Z9G2.I6GtJiRIe6QF!Mdb8Eep,GJI9.NXA1RL"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId), // Use user ID as the subject
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "FACES",
            audience: "FACES",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30), // Token expiration
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public int ExtractUserIdFromToken()
    {
        var token = ExtractTokenFromHeader();
        return int.Parse(ValidateAndExtractUserId(token));
    }

    public string ExtractTokenFromHeader()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var authHeader = httpContext?.Request.Headers["Authorization"].ToString();
        
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            return authHeader.Substring("Bearer ".Length).Trim();
        }
        return null;
    }

    public string ValidateAndExtractUserId(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

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

            // If the token is valid, extract claims
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            
            return userIdClaim?.Value; // Returns the user ID or null if not found
        }
        catch
        {
            // Handle token validation failure (like log the error)
            return null;
        }
    }
}
