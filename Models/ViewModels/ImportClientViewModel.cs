using System.ComponentModel.DataAnnotations;

namespace FACES.Models;
public class ClientImportModel
{
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public Gender Gender { get; set; }
    public string? Email { get; set; }
}
