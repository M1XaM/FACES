using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FACES.Models;
public class UserProject
{
    [ForeignKey("User")]
    public int UserId { get; set; }
    public required User User { get; set; }
    
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    public required Project Project { get; set; }
}
