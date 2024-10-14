public interface IJwtService
{
    string GenerateJwtToken(string userId);
    int ExtractUserIdFromToken();
    string ExtractTokenFromHeader();
    string ValidateAndExtractUserId(string token);
}
