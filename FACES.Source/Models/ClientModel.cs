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

public class Client
{
    [Key]
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }

    [EmailAddress]
    public required string Email { get; set; }
    public string? Number { get; set; }

	public CustomerType CustomerType { get; set; }
    
    // Navigation property
    public ICollection<ProjectClient> ProjectClients { get; set; } = new List<ProjectClient>();
}
