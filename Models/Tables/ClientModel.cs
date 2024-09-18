using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FACES.Models;
public enum Gender
{
    Male,
    Female,
    Rainbow
}

public enum CustomerType
{
	Regular,
	Premium
}

public class Client : IEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public Gender Gender { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }

    [EmailAddress]
    public string Email { get; set; }
    public string Number { get; set; }

    [Required]
	public CustomerType CustomerType { get; set; }
    
    // Navigation property
    public ICollection<ProjectClient> ProjectClients { get; set; } = new List<ProjectClient>();
}
