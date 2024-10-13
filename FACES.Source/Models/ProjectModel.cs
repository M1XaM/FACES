using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FACES.Models;
public class Project : IEntity
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; } 

    // Navigation property for the many-to-many relationship
    public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
    public ICollection<ProjectClient> ProjectClients { get; set; } = new List<ProjectClient>();
}
