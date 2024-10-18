using FACES.Repositories;
using FACES.Models;
using FACES.Data;

using Microsoft.EntityFrameworkCore;

namespace FACES.Repositories;
public class UserProjectRepository : GenericRepository<UserProject>, IUserProjectRepository
{

    public UserProjectRepository(ApplicationDbContext db) : base(db)
    {
    }

    public async Task<List<Project>?> GetProjectsByUserIdAsync(int userId)
    {
        return await _db.Set<UserProject>()
            .Where(up => up.UserId == userId)
            .Select(up => up.Project)
            .ToListAsync();
    }

    public async Task<Project?> GetProjectByUserIdAndProjectIdAsync(int userId, int projectId)
    {
        var userProjectRelation = await _db.UserProjects.FirstOrDefaultAsync(up => up.UserId == userId && up.ProjectId == projectId);
        return userProjectRelation == null ? null : await _db.Set<Project>().FirstOrDefaultAsync(p => p.Id == userProjectRelation.ProjectId);
    }
}