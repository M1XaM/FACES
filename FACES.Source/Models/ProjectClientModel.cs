using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FACES.Models;
public class ProjectClient
{
	[ForeignKey("Project")]
	public int ProjectId { get; set; }
	public required Project Project { get; set; }

	[ForeignKey("Client")]
	public int ClientId { get; set; }
	public required Client Client { get; set; }
}
