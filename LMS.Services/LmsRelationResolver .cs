using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Service.Contracts;

namespace LMS.Services;

public sealed class LmsRelationResolver : ILmsRelationResolver
{
    private readonly IUnitOfWork _unitOfWork;

    public LmsRelationResolver(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Course> GetCourseByIdAsync(int courseId, CancellationToken ct = default)
    {
        return await _unitOfWork.Courses.GetCourseAsync(courseId)
            ?? throw new NotFoundException($"Course with id {courseId} was not found.");
    }

    public async Task<Course> GetCourseFromModuleIdAsync(int moduleId, CancellationToken ct = default)
    {
        var module = await _unitOfWork.Modules.GetModuleAsync(moduleId)
            ?? throw new NotFoundException($"Module with id {moduleId} was not found.");

        return module.Course
            ?? throw new InvalidOperationException(
                $"Module with id {moduleId} was loaded without its Course relation.");
    }

    public async Task<Course> GetCourseFromActivityIdAsync(int activityId, CancellationToken ct = default)
    {
        var activity = await _unitOfWork.Activities.GetActivityWithRelationsAsync(activityId)
            ?? throw new NotFoundException($"Activity with id {activityId} was not found.");

        return activity.Module?.Course
            ?? throw new InvalidOperationException(
                $"Activity with id {activityId} was loaded without Module.Course relation.");
    }

    public async Task<Course> GetCourseFromSubmissionIdAsync(int submissionId, CancellationToken ct = default)
    {
        var submission = await _unitOfWork.Submissions.GetSubmissionWithRelationsAsync(submissionId)
            ?? throw new NotFoundException($"Submission with id {submissionId} was not found.");

        return submission.Activity?.Module?.Course
            ?? throw new InvalidOperationException(
                $"Submission with id {submissionId} was loaded without Activity.Module.Course relation.");
    }

    public Course? ResolveCourse(Module module)
    {
        return module.Course;
    }

    public Course? ResolveCourse(Activity activity)
    {
        return activity.Module?.Course;
    }

    public Course? ResolveCourse(Submission submission)
    {
        return submission.Activity?.Module?.Course;
    }

    public Course? ResolveCourse(Document document)
    {
        return document.Course
            ?? document.Module?.Course
            ?? document.Activity?.Module?.Course
            ?? document.Submission?.Activity?.Module?.Course;
    }
}