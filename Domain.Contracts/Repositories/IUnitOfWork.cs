namespace Domain.Contracts.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
    ICourseRepository Courses { get; }
    IModuleRepository Modules { get; }
}