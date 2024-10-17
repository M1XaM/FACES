using FACES.Repositories;
using FACES.Models;

namespace FACES.Repositories;
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUserByEmail(string email);
}