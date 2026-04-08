using Domain.Models.Entities;

namespace Service.Contracts;

/// <summary>
/// Central service for LMS access control.
/// </summary>
public interface ILmsAccessService
{
    /// <summary>
    /// Ensures that the user has teacher-level access to the specified course.
    /// </summary>
    Task EnsureTeacherForCourseAsync(string userId, Course course, CancellationToken ct = default);

    /// <summary>
    /// Ensures that the user may access the specified document.
    /// </summary>
    Task EnsureCanAccessDocumentAsync(string userId, Document document, CancellationToken ct = default);
    Task EnsureCanModifyDocumentAsync(string userId, Document document, CancellationToken ct = default);
    Task EnsureCanDeleteDocumentAsync(string userId, Document document, CancellationToken ct = default);

    /// <summary>
    /// Ensures that the user may access the specified course.
    /// </summary>
    Task EnsureCanAccessCourseAsync(string userId, Course course, CancellationToken ct = default);


    /// <summary>
    /// Ensures that the user may access the specified module.
    /// </summary>
    Task EnsureCanAccessModuleAsync(string userId, Module module, CancellationToken ct = default);

    /// <summary>
    /// Ensures that the user may access the specified activity.
    /// </summary>
    Task EnsureCanAccessActivityAsync(string userId, Activity activity, CancellationToken ct = default);

    /// <summary>
    /// Ensures that the user may access the specified submission.
    /// </summary>
    Task EnsureCanAccessSubmissionAsync(string userId, Submission submission, CancellationToken ct = default);

    /// <summary>
    /// Applies access filtering for documents.
    /// </summary>
    IQueryable<Document> ApplyDocumentAccessFilter(IQueryable<Document> query, IUserAccessContext access);

    /// <summary>
    /// Applies access filtering for courses.
    /// </summary>
    IQueryable<Course> ApplyCourseAccessFilter(IQueryable<Course> query, IUserAccessContext access);

    /// <summary>
    /// Applies access filtering for modules.
    /// </summary>
    IQueryable<Module> ApplyModuleAccessFilter(IQueryable<Module> query, IUserAccessContext access);

    /// <summary>
    /// Applies access filtering for activities.
    /// </summary>
    IQueryable<Activity> ApplyActivityAccessFilter(IQueryable<Activity> query, IUserAccessContext access);

    /// <summary>
    /// Applies access filtering for submissions.
    /// </summary>
    IQueryable<Submission> ApplySubmissionAccessFilter(IQueryable<Submission> query, IUserAccessContext access);
}