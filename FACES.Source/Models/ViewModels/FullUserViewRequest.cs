using System.ComponentModel.DataAnnotations;

namespace FACES.RequestModels;
public class FullUserViewRequest
{
    [Required]
    public required string FirstName { get; set; }
    [Required]
    public required string LastName { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}