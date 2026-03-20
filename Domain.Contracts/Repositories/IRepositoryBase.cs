namespace Domain.Contracts.Repositories;

public interface IRepositoryBase<T>
{
    void Update(T entity);
    void Delete(T entity);
    void Create(T entity);

    Task<T> CreateAsync(T entity);
    Task<T?> FindByIdAsync(int? id);
}