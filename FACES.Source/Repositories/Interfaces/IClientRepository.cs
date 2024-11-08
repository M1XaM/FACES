using FACES.Repositories;
using FACES.Models;

namespace FACES.Repositories;
public interface IClientRepository : IGenericRepository<Client>
{
    Task<Client?> GetClientByEmailAsync(string email);
}