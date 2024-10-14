namespace FACES.ResponseModels;
public class AuthResponse
{
    public required bool Success { get; set; }
    public string? Message { get; set; } = null;
    public string? Token { get; set; } = null;
}