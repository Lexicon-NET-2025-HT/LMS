namespace Domain.Contracts.Repositories;

public interface IRepositoryBase<T>
{
    void Create(T entity);
    Task<T?> FindByIdAsync(int? id);
    Task<bool> ExistsAsync(int id);
    void Update(T entity);
    void Delete(T entity);
}