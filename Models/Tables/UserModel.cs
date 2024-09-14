using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FACES.Models;
// see more about relationshipsin databases on https://learn.microsoft.com/en-us/ef/core/modeling/relationships
public class User 
{
    [Key]  // see more annotations on https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-8.0
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public DateTime? CreationTime { get; set; } = DateTime.UtcNow;

    // Navigation property for the many-to-many relationship
    public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
}
