using System.ComponentModel.DataAnnotations;

namespace FACES.RequestModels;
public class LoginViewRequest
{
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}