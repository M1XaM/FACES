using System.ComponentModel.DataAnnotations;

namespace FACES.RequestModels;
public class ProjectViewModel
{
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
}