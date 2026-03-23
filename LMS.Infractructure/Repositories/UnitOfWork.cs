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
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IDocumentRepository Documents { get; } = documentRepository;
    public ICourseRepository Courses { get; } = courseRepository;
    public IModuleRepository Modules { get; } = moduleRepository;
    public IActivityRepository Activities { get; } = activityRepository;

    public async Task CompleteAsync() => await _context.SaveChangesAsync();
}