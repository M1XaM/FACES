using FACES.RequestModels;
using FACES.ResponseModels;

public interface IProjectService
{
    Task<ProjectResponse> GetUserProjects();
    Task<ProjectResponse> CreateProject(ProjectRequest projectRequest);
    // Task<ProjectResponse> ModifyProject();
    // Task<ProjectResponse> DeleteProejct();
}