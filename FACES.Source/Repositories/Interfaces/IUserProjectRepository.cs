using FACES.Repositories;
using FACES.RequestModels;
using FACES.Models;

namespace FACES.Repositories;
public interface IUserProjectRepository : IGenericRepository<UserProject>
{
    Task<List<Project>?> GetProjectsByUserIdAsync(int userId);
    Task<Project?> GetProjectByUserIdAndProjectIdAsync(int userId, int projectId);
}