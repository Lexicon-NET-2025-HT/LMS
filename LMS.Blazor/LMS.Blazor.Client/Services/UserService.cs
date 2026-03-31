using LMS.Shared.DTOs.User;

namespace LMS.Blazor.Client.Services;

public class UserService(IApiService apiService) : IUserService
{
    private readonly IApiService _apiService = apiService;
    private const string Base = "api/users";

    public Task<List<StudentBasicDto>?> GetAllUsersAsync(CancellationToken ct = default)
        => _apiService.GetAsync<List<StudentBasicDto>>(Base, ct);

    public Task<List<StudentBasicDto>?> GetUsersWithoutCourseAsync(CancellationToken ct = default)
        => _apiService.GetAsync<List<StudentBasicDto>>($"{Base}/without-course", ct);

    public Task<List<StudentDto>?> GetUsersByCourseAsync(int courseId, CancellationToken ct = default)
        => _apiService.GetAsync<List<StudentDto>>($"{Base}/course/{courseId}", ct);

    public Task<List<StudentBasicDto>?> GetTeachersAsync(CancellationToken ct = default)
        => _apiService.GetAsync<List<StudentBasicDto>>($"{Base}/teachers", ct);

    public async Task<bool> EnrollUserInCourseAsync(string userId, int courseId, CancellationToken ct = default)
    {
        var result = await _apiService.PutAsync<object>($"{Base}/{userId}/enroll/{courseId}", new { }, ct);
        return result is not null;
    }

    public async Task<bool> RemoveUserFromCourseAsync(string userId, CancellationToken ct = default)
    {
        var result = await _apiService.PutAsync<object>($"{Base}/{userId}/remove-course", new { }, ct);
        return result is not null;
    }

    public Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default)
        => _apiService.DeleteAsync($"{Base}/{userId}", ct);
}