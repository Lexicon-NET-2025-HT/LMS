using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

public interface ISubmissionRepository : IRepositoryBase<Submission>
{
    Task<(IEnumerable<Submission> Submissions, int TotalCount)> GetAllSubmissionsAsync(
        int page, int pageSize, int? activityId, bool trackChanges = false);
    Task<Submission?> GetSubmissionAsync(int id, bool trackChanges = false);
}