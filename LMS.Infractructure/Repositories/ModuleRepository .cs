using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Infractructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories;

public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
{
    public ModuleRepository(ApplicationDbContext context) : base(context) { }

    public async Task<(IEnumerable<Module> Modules, int TotalCount)> GetAllModulesAsync(
        int page, int pageSize, int? courseId, bool trackChanges = false)
    {
        var query = FindByCondition(
            m => courseId == null || m.CourseId == courseId.Value,
            trackChanges)
            .Include(m => m.Course)
            .Include(m => m.Activities);

        return await query.PagedResult(page, pageSize);
    }
    public async Task<Module?> GetModuleAsync(int id, bool trackChanges = false)
    {
        return await FindByCondition(m => m.Id == id, trackChanges)
            .Include(m => m.Activities)
            .Include(m => m.Documents)
            .FirstOrDefaultAsync();
    }
}