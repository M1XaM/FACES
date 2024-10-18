using FACES.Repositories;
using FACES.Models;
using FACES.Data;

using Microsoft.EntityFrameworkCore;

namespace FACES.Repositories;
public class ProjectClientRepository : GenericRepository<ProjectClient>, IProjectClientRepository
{

    public ProjectClientRepository(ApplicationDbContext db) : base(db)
    {
    }

    public async Task<List<Client>> GetClientsByProjectIdAsync(int projectId)
    {
        return await _db.Set<ProjectClient>()
            .Where(pc => pc.ProjectId == projectId)
            .Select(pc => pc.Client)
            .ToListAsync();
    }
}