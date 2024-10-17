using FACES.Models;

namespace FACES.ResponseModels;
public class UserActionResponse
{
    public required bool Success { get; set; }
    public string? Message { get; set; } = null;
    public User? User { get; set; } = null;
}