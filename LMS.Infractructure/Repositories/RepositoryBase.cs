using Domain.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LMS.Infractructure.Repositories;

public abstract class RepositoryBase<T>(DbSet<T> dbSet) : IRepositoryBase<T>, IInternalRepositoryBase<T> where T : class //Do Entitybase
{
    protected DbSet<T> DbSet { get; } = dbSet;

    public void Create(T entity) => DbSet.AddAsync(entity);
    public async Task<T?> FindByIdAsync(int? id) => await DbSet.FindAsync(id);
    public void Update(T entity) => DbSet.Update(entity);
    public void Delete(T entity) => DbSet.Remove(entity);

    public IQueryable<T> FindAll(bool trackChanges = false)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        throw new NotImplementedException();
    }
}