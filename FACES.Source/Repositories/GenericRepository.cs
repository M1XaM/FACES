using Microsoft.EntityFrameworkCore;
using FACES.Models;
using FACES.Data;


namespace FACES.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private ApplicationDbContext _db;
        DbSet<T> _entity = null;

        public GenericRepository(ApplicationDbContext db)
        {
            _db = db;
            _entity = _db.Set<T>();
        }

        public bool Add(T model)
        {
            try
            {
                _entity.Add(model);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(T model)
        {
            try
            {
                _entity.Update(model);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(T model)
        {
            try
            {
                var obj = _entity.Find(model.Id);
                if (obj != null)
                {
                    _entity.Remove(obj);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public T GetById(int id)
        {
            try
            {
                return _entity.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _entity.ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
