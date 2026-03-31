using LMS.Shared.DTOs.User;

namespace LMS.Blazor.Client.Services;

public interface IUserService
{
    Task<List<StudentBasicDto>?> GetAllUsersAsync(CancellationToken ct = default);
    Task<List<StudentBasicDto>?> GetUsersWithoutCourseAsync(CancellationToken ct = default);
    Task<List<StudentDto>?> GetUsersByCourseAsync(int courseId, CancellationToken ct = default);
    Task<List<StudentBasicDto>?> GetTeachersAsync(CancellationToken ct = default);
    Task<bool> EnrollUserInCourseAsync(string userId, int courseId, CancellationToken ct = default);
    Task<bool> RemoveUserFromCourseAsync(string userId, CancellationToken ct = default);
    Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default);
}
