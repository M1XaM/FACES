using System.ComponentModel.DataAnnotations;

namespace FACES.RequestModels;
public class ProjectViewRequest
{
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
}