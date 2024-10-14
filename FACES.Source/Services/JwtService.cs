using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _jwtKey;

    public JwtService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _config = config;
        _httpContextAccessor = httpContextAccessor;
        _jwtKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured. Please set the 'Jwt:Key' in the configuration.");
    }

    public string GenerateJwtToken(string userId)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtKey));
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
        throw new InvalidOperationException($"Jwt Service Error: token extraction error)");
    }

    public string ValidateAndExtractUserId(string token)
    {
        if (string.IsNullOrEmpty(token)) throw new InvalidOperationException($"Jwt Service Error: token is null or empty)");;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtKey);

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

            if (userIdClaim == null) throw new InvalidOperationException($"Jwt Service Error: id from token is null");
            return userIdClaim.Value;
        }
        catch
        {
            throw new InvalidOperationException($"Jwt Service Error: token validation error");
        }
    }
}
