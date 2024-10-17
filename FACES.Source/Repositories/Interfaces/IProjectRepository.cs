using FACES.Repositories;
using FACES.RequestModels;
using FACES.Models;

namespace FACES.Repositories;
public interface IProjectRepository : IGenericRepository<Project>
{
    Task<Project?> GetProjectByName(string projectName);
}