namespace AgendaPro.Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    public Task<T?> GetByIdAsync(Guid id);
    public Task<List<T>> GetAllAsync();
    public Task AddAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task DeleteAsync(Guid id);
}