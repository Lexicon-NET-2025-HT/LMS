using LMS.Shared.DTOs.Activity;
using LMS.Shared.DTOs.Common;

namespace LMS.Blazor.Client.Services;

public class ActivityService(IApiService apiService) : IActivityService
{
    private const string Base = "api/activities";

    public async Task<PagedResultDto<ActivityDto>?> GetAllActivitiesAsync(
        int page = 1, 
        int pageSize = 10,
        int? moduleId = null,
        CancellationToken ct = default)
    {
        return await apiService.GetAsync<PagedResultDto<ActivityDto>>($"{Base}?page={page}&pageSize={pageSize}", ct);
    }

    public async Task<PagedResultDto<ActivityDto>> GetAllActivitiesByModuleAsync(
        int moduleId,
        int page = 1, 
        int pageSize = 10, 
        CancellationToken ct = default)
    {
        var pagedResult = await apiService.GetAsync<PagedResultDto<ActivityDto>>(
            $"{Base}?page={page}&pageSize={pageSize}&moduleId={moduleId}", ct);

        return pagedResult!;
    }
}