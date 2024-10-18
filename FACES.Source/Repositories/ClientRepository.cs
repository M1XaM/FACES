using FACES.Repositories;
using FACES.Models;
using FACES.Data;

using Microsoft.EntityFrameworkCore;

namespace FACES.Repositories;
public class ClientRepository : GenericRepository<Client>, IClientRepository
{

    public ClientRepository(ApplicationDbContext db) : base(db)
    {
    }

    public async Task<Client?> GetClientByEmailAsync(string email)
    {
        return await _db.Set<Client>().SingleOrDefaultAsync(c => c.Email == email);
    }
}