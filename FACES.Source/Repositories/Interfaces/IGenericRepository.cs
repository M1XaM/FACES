namespace FACES.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<bool> AddAsync(T model);
        Task<bool> UpdateAsync(T model);
        Task<bool> DeleteAsync(T model);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}