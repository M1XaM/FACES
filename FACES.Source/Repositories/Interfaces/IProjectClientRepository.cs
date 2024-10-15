using FACES.Repositories;
using FACES.RequestModels;
using FACES.Models;

namespace FACES.Repositories;
public interface IProjectClientRepository :  IGenericRepository<ProjectClient>
{
    Task<List<Client>> GetClientsByProjectId(int projectId);
}