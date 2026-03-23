using Domain.Contracts.Repositories;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;

public class UnitOfWork(
    ApplicationDbContext context,
    ICourseRepository courseRepository,
    IModuleRepository moduleRepository,
    IActivityRepository activityRepository,
    IDocumentRepository documentRepository) : IUnitOfWork
{

    

    public async Task CompleteAsync() => await _context.SaveChangesAsync();
}