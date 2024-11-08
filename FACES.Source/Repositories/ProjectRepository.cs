using FACES.RequestModels;
using FACES.Repositories;
using FACES.Models;
using FACES.Data;

using Microsoft.EntityFrameworkCore;

namespace FACES.Repositories;
public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{

    public ProjectRepository(ApplicationDbContext db) : base(db)
    {
    }

    public async Task<Project?> GetProjectByNameAsync(string projectName)
    {
        return await _db.Set<Project>().SingleOrDefaultAsync(p => p.Name == projectName);
    }
}