namespace Domain.Contracts.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
    ICourseRepository Courses { get; }
}