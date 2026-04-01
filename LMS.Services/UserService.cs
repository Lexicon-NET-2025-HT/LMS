// LMS.Services/UserService.cs
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Shared.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;

namespace LMS.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;

    public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    // ── helpers ───────────────────────────────────────────────────────────────

    private async Task<string?> GetRoleIdAsync(string roleName, CancellationToken ct) =>
        await _db.Roles
            .Where(r => r.Name == roleName)
            .Select(r => r.Id)
            .FirstOrDefaultAsync(ct);

    private IQueryable<ApplicationUser> UsersInRole(string roleId) =>
        _db.Users.Where(u =>
            _db.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == roleId));

    private static StudentBasicDto MapToDto(ApplicationUser u) => new()
    {
        Id = u.Id,
        UserName = u.UserName ?? string.Empty,
        Email = u.Email ?? string.Empty,
        CourseId = u.CourseId,
        CourseName = u.Course?.Name
    };

    // ── queries ───────────────────────────────────────────────────────────────

    public async Task<IEnumerable<StudentBasicDto>> GetAllStudentsAsync(CancellationToken ct = default)
    {
        var roleId = await GetRoleIdAsync("Student", ct);
        if (roleId is null) return [];

        var users = await UsersInRole(roleId)
            .Include(u => u.Course)
            .OrderBy(u => u.Email)
            .ToListAsync(ct);

        return users.Select(MapToDto);
    }

    public async Task<StudentBasicDto?> GetUserByIdAsync(string id, CancellationToken ct = default)
    {
        var user = await _db.Users
            .Include(u => u.Course)
            .FirstOrDefaultAsync(u => u.Id == id, ct);

        return user is null ? null : MapToDto(user);
    }

    public async Task<IEnumerable<StudentDto>> GetUsersByCourseAsync(int courseId, CancellationToken ct = default)
    {
        return await _db.Users
            .Where(u => u.CourseId == courseId)
            .OrderBy(u => u.UserName)
            .Select(u => new StudentDto
            {
                Id = u.Id,
                Name = u.UserName ?? string.Empty,
                Email = u.Email ?? string.Empty
            })
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<StudentBasicDto>> GetUsersWithoutCourseAsync(CancellationToken ct = default)
    {
        var roleId = await GetRoleIdAsync("Student", ct);
        if (roleId is null) return [];

        var users = await UsersInRole(roleId)
            .Where(u => u.CourseId == null)
            .OrderBy(u => u.Email)
            .ToListAsync(ct);

        return users.Select(MapToDto);
    }

    public async Task<IEnumerable<StudentBasicDto>> GetTeachersAsync(CancellationToken ct = default)
    {
        var roleId = await GetRoleIdAsync("Teacher", ct);
        if (roleId is null) return [];

        return await UsersInRole(roleId)
            .OrderBy(u => u.Email)
            .Select(u => new StudentBasicDto
            {
                Id = u.Id,
                UserName = u.UserName ?? string.Empty,
                Email = u.Email ?? string.Empty,
                CourseId = null,
                CourseName = null
            })
            .ToListAsync(ct);
    }

    // ── commands ──────────────────────────────────────────────────────────────

    public async Task EnrollUserInCourseAsync(string userId, int courseId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException($"User '{userId}' was not found.");

        var courseExists = await _db.Courses.AnyAsync(c => c.Id == courseId, ct);
        if (!courseExists)
            throw new KeyNotFoundException($"Course '{courseId}' was not found.");

        user.CourseId = courseId;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task RemoveUserFromCourseAsync(string userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException($"User '{userId}' was not found.");

        user.CourseId = null;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task DeleteUserAsync(string currentUserId, string targetUserId, CancellationToken ct = default)
    {
        if (currentUserId == targetUserId)
            throw new InvalidOperationException("You cannot delete your own account.");

        var user = await _userManager.FindByIdAsync(targetUserId)
            ?? throw new KeyNotFoundException($"User '{targetUserId}' was not found.");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}