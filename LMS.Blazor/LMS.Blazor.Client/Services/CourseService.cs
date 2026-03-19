using LMS.Shared.DTOs.Course;
using System.Text.Json;

namespace LMS.Blazor.Client.Services;

public class CourseService : ICourseService
{
    private readonly IApiService _apiService;
    private readonly ILogger<CourseService> _logger;

    private const string Base = "courses";

    public CourseService(IApiService apiService, ILogger<CourseService> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<List<CourseDto>> GetAllCoursesAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _apiService.GetAsync<List<CourseDto>>(Base, ct);
            return result ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error fetching all courses.");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize courses response.");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Request timed out fetching all courses.");
        }
        return [];
    }

    public async Task<CourseDto?> GetCourseAsync(int courseId, CancellationToken ct = default)
    {
        try
        {
            return await _apiService.GetAsync<CourseDto>($"{Base}/{courseId}", ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error fetching course {CourseId}.", courseId);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize course {CourseId} response.", courseId);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Request timed out fetching course {CourseId}.", courseId);
        }
        return null;
    }

    public async Task<CourseDto?> CreateCourseAsync(CreateCourseDto dto, CancellationToken ct = default)
    {
        try
        {
            return await _apiService.PostAsync<CourseDto>(Base, dto, ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error creating course.");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize create course response.");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Request timed out creating course.");
        }
        return null;
    }

    public async Task<CourseDto?> UpdateCourseAsync(int courseId, UpdateCourseDto dto, CancellationToken ct = default)
    {
        try
        {
            return await _apiService.PutAsync<CourseDto>($"{Base}/{courseId}", dto, ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error updating course {CourseId}.", courseId);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize update course {CourseId} response.", courseId);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Request timed out updating course {CourseId}.", courseId);
        }
        return null;
    }

    public async Task<bool> DeleteCourseAsync(int courseId, CancellationToken ct = default)
    {
        try
        {
            return await _apiService.DeleteAsync($"{Base}/{courseId}", ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error deleting course {CourseId}.", courseId);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Request timed out deleting course {CourseId}.", courseId);
        }
        return false;
    }
}