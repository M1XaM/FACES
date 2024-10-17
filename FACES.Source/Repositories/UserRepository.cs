using FACES.Repositories;
using FACES.Models;
using FACES.Data;

using Microsoft.EntityFrameworkCore;

namespace FACES.Repositories;
public class UserRepository : GenericRepository<User>, IUserRepository
{

    public UserRepository(ApplicationDbContext db) : base(db)
    {
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _db.Set<User>().SingleOrDefaultAsync(u => u.Email == email);
    }
}