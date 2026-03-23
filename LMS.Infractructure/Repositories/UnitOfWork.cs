using Domain.Contracts.Repositories;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;

    public ICourseRepository Courses { get; }
    public IModuleRepository Modules { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        Courses = new CourseRepository(context);
        Modules = new ModuleRepository(context);
    }

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}