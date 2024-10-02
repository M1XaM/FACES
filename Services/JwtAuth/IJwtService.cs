using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public interface IJwtService
{
    string GenerateJwtToken(string userId);
    int ExtractUserIdFromToken();
    string ExtractTokenFromHeader();
    string ValidateAndExtractUserId(string token);
}
