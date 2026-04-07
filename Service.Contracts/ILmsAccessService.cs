using Domain.Models.Entities;

namespace Service.Contracts;

public interface ILmsAccessService
{
    Task EnsureTeacherForCourseAsync(string userId, Course course, CancellationToken ct = default);
    Task EnsureCanAccessDocumentAsync(string userId, Document document, CancellationToken ct = default);
    Task EnsureCanAccessCourseAsync(string userId, Course course, CancellationToken ct = default);
    Task EnsureCanAccessModuleAsync(string userId, Module module, CancellationToken ct = default);
    Task EnsureCanAccessActivityAsync(string userId, Activity activity, CancellationToken ct = default);
    Task EnsureCanAccessSubmissionAsync(string userId, Submission submission, CancellationToken ct = default);

    IQueryable<Document> ApplyDocumentAccessFilter(IQueryable<Document> query, string userId, bool isTeacher, int? studentCourseId, List<int> teacherCourseIds);
    IQueryable<Course> ApplyCourseAccessFilter(IQueryable<Course> query, string userId, bool isTeacher, int? studentCourseId, List<int> teacherCourseIds);
    IQueryable<Module> ApplyModuleAccessFilter(IQueryable<Module> query, string userId, bool isTeacher, int? studentCourseId, List<int> teacherCourseIds);
    IQueryable<Activity> ApplyActivityAccessFilter(IQueryable<Activity> query, string userId, bool isTeacher, int? studentCourseId, List<int> teacherCourseIds);
    IQueryable<Submission> ApplySubmissionAccessFilter(IQueryable<Submission> query, string userId, bool isTeacher, int? studentCourseId, List<int> teacherCourseIds);
}