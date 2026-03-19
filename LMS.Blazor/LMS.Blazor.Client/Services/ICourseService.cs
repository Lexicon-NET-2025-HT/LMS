using LMS.Shared.DTOs.Course;

namespace LMS.Blazor.Client.Services;

public interface ICourseService
{
    Task<List<CourseDto>> GetAllCoursesAsync(CancellationToken ct = default);
    Task<CourseDto?> GetCourseAsync(int courseId, CancellationToken ct = default);
    Task<CourseDto?> CreateCourseAsync(CreateCourseDto dto, CancellationToken ct = default);
    Task<CourseDto?> UpdateCourseAsync(int courseId, UpdateCourseDto dto, CancellationToken ct = default);
    Task<bool> DeleteCourseAsync(int courseId, CancellationToken ct = default);
}