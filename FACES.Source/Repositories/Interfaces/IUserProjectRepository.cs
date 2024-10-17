using FACES.Repositories;
using FACES.RequestModels;
using FACES.Models;

namespace FACES.Repositories;
public interface IUserProjectRepository : IGenericRepository<UserProject>
{
    Task<List<Project>?> GetProjectsByUserId(int userId);
    Task<Project?> GetProjectByUserIdAndProjectId(int userId, int projectId);
}