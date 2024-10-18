using FACES.Repositories;
using FACES.RequestModels;
using FACES.Models;

namespace FACES.Repositories;
public interface IProjectRepository : IGenericRepository<Project>
{
    Task<Project?> GetProjectByNameAsync(string projectName);
}