using LMS.Shared.DTOs.Activity;
using LMS.Shared.DTOs.Common;

namespace LMS.Blazor.Client.Services;

public interface IActivityService
{
    Task<PagedResultDto<ActivityDto>?> GetAllActivitiesAsync(int page = 1, int pageSize = 10, int? moduleId = null, CancellationToken ct = default);
    Task<PagedResultDto<ActivityDto>> GetAllActivitiesByModuleAsync(int moduleId, int page = 1, int pageSize = 10, CancellationToken ct = default);
}