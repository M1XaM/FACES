namespace FACES.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        bool Add(T model);
        bool Update(T model);
        bool Delete(T model);
        T GetById(int id);
        IEnumerable<T> GetAll();
        
    }
}