

public interface IJwtService
{
    string GenerateJwtToken(string userId);
    int ExtractUserIdFromToken();
    string ExtractTokenFromHeader();
    int ValidateAndExtractUserId(string token);
    bool TokenVerification();
}
