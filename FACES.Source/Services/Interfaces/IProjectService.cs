using FACES.RequestModels;
using FACES.ResponseModels;

public interface IProjectService
{
    Task<ProjectServiceResponse> GetUserProjectsAsync();
    Task<ProjectServiceResponse> CreateProjectAsync(ProjectViewModel projectRequest);
    // Task<ProjectResponse> ModifyProject();
    // Task<ProjectResponse> DeleteProejct();
}