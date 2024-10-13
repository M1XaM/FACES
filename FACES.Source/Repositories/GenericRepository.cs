using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FACES.Models;
using FACES.Data;

namespace FACES.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _entity;

        public GenericRepository(ApplicationDbContext db)
        {
            _db = db;
            _entity = _db.Set<T>();
        }

        public async Task<bool> AddAsync(T model)
        {
            try
            {
                await _entity.AddAsync(model);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T model)
        {
            try
            {
                _entity.Update(model);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(T model)
        {
            try
            {
                var obj = await _entity.FindAsync(model.Id);
                if (obj != null)
                {
                    _entity.Remove(obj);
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _entity.FindAsync(id);
                if (entity == null)
                {
                    throw new KeyNotFoundException($"Entity with id {id} not found.");
                }
                return entity;
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"This operation is not allowed in the current state: {ex}");
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _entity.ToListAsync();
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"This operation is not allowed in the current state: {ex}");
            }
        }
    }
}
