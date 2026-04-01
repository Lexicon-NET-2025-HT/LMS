using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

public interface IActivityRepository : IRepositoryBase<Activity>
{
    Task<Activity?> GetActivity(int id, bool trackChanges = false);
}