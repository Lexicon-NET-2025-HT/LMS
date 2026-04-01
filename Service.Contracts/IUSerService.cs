// Service.Contracts/IUserService.cs
using LMS.Shared.DTOs.User;

namespace Service.Contracts;

public interface IUserService
{
    Task<IEnumerable<StudentBasicDto>> GetAllStudentsAsync(CancellationToken ct = default);
    Task<StudentBasicDto?> GetUserByIdAsync(string id, CancellationToken ct = default);
    Task<IEnumerable<StudentDto>> GetUsersByCourseAsync(int courseId, CancellationToken ct = default);
    Task<IEnumerable<StudentBasicDto>> GetUsersWithoutCourseAsync(CancellationToken ct = default);
    Task<IEnumerable<StudentBasicDto>> GetTeachersAsync(CancellationToken ct = default);
    Task EnrollUserInCourseAsync(string userId, int courseId, CancellationToken ct = default);
    Task RemoveUserFromCourseAsync(string userId, CancellationToken ct = default);
    Task DeleteUserAsync(string currentUserId, string targetUserId, CancellationToken ct = default);
}