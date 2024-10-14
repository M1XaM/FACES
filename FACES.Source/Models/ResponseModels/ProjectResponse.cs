using FACES.Models;

namespace FACES.ResponseModels;
public class ProjectResponse
{
    public required bool Success { get; set; }
    public string? Message { get; set; } = null;
    public List<Project>? Projects { get; set; } = null;
}