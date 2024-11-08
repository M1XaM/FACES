using FACES.Models;

namespace FACES.ResponseModels;
public class ProjectServiceResponse : ServiceResponse
{
    public List<Project>? Projects { get; set; } = null;
}