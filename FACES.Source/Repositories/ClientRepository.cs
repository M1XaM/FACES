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

}