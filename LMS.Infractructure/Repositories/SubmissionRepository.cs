using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Infractructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories;

public class SubmissionRepository(ApplicationDbContext context) : RepositoryBase<Submission>(context), ISubmissionRepository
{
    public async Task<(IEnumerable<Submission> Submissions, int TotalCount)> GetAllSubmissionsAsync(
        int page, int pageSize, int? activityId, bool trackChanges = false)
    {
        var query = FindByCondition(
            s => activityId == null || s.ActivityId == activityId.Value,
            trackChanges)
            .Include(s => s.Student)
            .Include(s => s.Activity)
            .Include(s => s.Document);

        return await query.PagedResult(page, pageSize);
    }
    public async Task<Submission?> GetSubmissionAsync(int id, bool trackChanges = false)
    {
        return await FindByCondition(m => m.Id == id, trackChanges)
            .Include(s => s.Student)
            .Include(s => s.Activity)
            .Include(s => s.Document)
            .FirstOrDefaultAsync();
    }
}