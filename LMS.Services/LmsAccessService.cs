using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Service.Contracts;

namespace LMS.Services;

public class LmsAccessService : ILmsAccessService
{
    private readonly ILmsRelationResolver _resolver;

    public LmsAccessService(ILmsRelationResolver resolver)
    {
        _resolver = resolver;
    }

    public Task EnsureTeacherForCourseAsync(string userId, Course course, CancellationToken ct = default)
    {
        if (!IsTeacherForCourse(userId, course))
            throw new ForbiddenException("Only teachers for this course are allowed for this operation.");

        return Task.CompletedTask;
    }

    public Task EnsureCanAccessCourseAsync(string userId, Course course, CancellationToken ct = default)
    {
        if (!CanAccessCourse(userId, course))
            throw new NotFoundException($"Course with id {course.Id} does not exist.");

        return Task.CompletedTask;
    }

    public Task EnsureCanAccessModuleAsync(string userId, Module module, CancellationToken ct = default)
    {
        var course = _resolver.ResolveCourse(module);

        if (course == null || !CanAccessCourse(userId, course))
            throw new NotFoundException($"Module with id {module.Id} does not exist.");

        return Task.CompletedTask;
    }

    public Task EnsureCanAccessActivityAsync(string userId, Activity activity, CancellationToken ct = default)
    {
        var course = _resolver.ResolveCourse(activity);

        if (course == null || !CanAccessCourse(userId, course))
            throw new NotFoundException($"Activity with id {activity.Id} does not exist.");

        return Task.CompletedTask;
    }

    public Task EnsureCanAccessSubmissionAsync(string userId, Submission submission, CancellationToken ct = default)
    {
        var course = _resolver.ResolveCourse(submission);

        var isOwner = submission.StudentId == userId;
        var isTeacher = course != null && IsTeacherForCourse(userId, course);
        var isStudentInCourse = submission.Student?.CourseId == course?.Id && submission.StudentId == userId;

        if (!isOwner && !isTeacher)
            throw new NotFoundException($"Submission with id {submission.Id} does not exist.");

        return Task.CompletedTask;
    }

    public Task EnsureCanAccessDocumentAsync(string userId, Document document, CancellationToken ct = default)
    {
        if (document.UploadedByUserId == userId)
            return Task.CompletedTask;

        var course = _resolver.ResolveCourse(document);

        if (document.SubmissionId != null)
        {
            var isOwner = document.Submission?.StudentId == userId;
            var isTeacher = course != null && IsTeacherForCourse(userId, course);

            if (!isOwner && !isTeacher)
                throw new NotFoundException($"Document with id {document.Id} does not exist.");

            return Task.CompletedTask;
        }

        if (course == null || !CanAccessCourse(userId, course))
            throw new NotFoundException($"Document with id {document.Id} does not exist.");

        return Task.CompletedTask;
    }

    private static bool IsTeacherForCourse(string userId, Course course) =>
        course.CourseTeachers.Any(ct => ct.TeacherId == userId);

    private static bool IsStudentForCourse(string userId, Course course) =>
        course.Students.Any(s => s.Id == userId);

    private static bool CanAccessCourse(string userId, Course course) =>
        IsTeacherForCourse(userId, course) || IsStudentForCourse(userId, course);

    public IQueryable<Document> ApplyDocumentAccessFilter(IQueryable<Document> query, string userId, bool isTeacher, int? studentCourseId, List<int> teacherCourseIds)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Course> ApplyCourseAccessFilter(IQueryable<Course> query, string userId, bool isTeacher, int? studentCourseId, List<int> teacherCourseIds)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Module> ApplyModuleAccessFilter(IQueryable<Module> query, string userId, bool isTeacher, int? studentCourseId, List<int> teacherCourseIds)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Activity> ApplyActivityAccessFilter(IQueryable<Activity> query, string userId, bool isTeacher, int? studentCourseId, List<int> teacherCourseIds)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Submission> ApplySubmissionAccessFilter(IQueryable<Submission> query, string userId, bool isTeacher, int? studentCourseId, List<int> teacherCourseIds)
    {
        throw new NotImplementedException();
    }

}
