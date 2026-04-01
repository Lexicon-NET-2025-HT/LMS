using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

public interface IActivityRepository : IRepositoryBase<Activity>
{
    Task<Activity?> FindByIdWithDetailAsync(int? id);
    Task<(IEnumerable<Activity> activities, int totalCount)> GetAllActivitiesAsync(
        int page, int pageSize, int? moduleId, bool trackChanges = false);
    Task<Activity?> GetActivity(int id, bool trackChanges = false);
}