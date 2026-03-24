using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories;

public class ActivityRepository(ApplicationDbContext context) : RepositoryBase<Activity>(context), IActivityRepository
{
    public async Task<Activity?> FindByIdWithDetailAsync(int? id)
    {
        return await DbSet
            .Include(a => a.Documents)
            .Include(a => a.Submissions)
            .FirstAsync(a => a.Id == id);
    }
}