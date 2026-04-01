using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories;

public class ActivityRepository(ApplicationDbContext context) : RepositoryBase<Activity>(context), IActivityRepository
{
    public async Task<Activity?> GetActivity(int id, bool trackChanges = false)
    {
        return await FindByCondition(a => a.Id == id, trackChanges)
            .Include(a => a.Module)
                .ThenInclude(m => m.Course)
            .Include(a => a.Documents)
            .Include(a => a.Submissions)
            .FirstOrDefaultAsync();
    }
}