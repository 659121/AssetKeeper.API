public interface IBaseRepository<T> where T : class
{
    Task<T> GetByIdAsync(int Guid);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int Guid);
}