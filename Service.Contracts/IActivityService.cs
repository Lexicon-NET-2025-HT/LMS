using LMS.Shared.DTOs.Activity;
using LMS.Shared.DTOs.Common;
using LMS.Shared.Request;

namespace Service.Contracts
{
    /// <summary>
    /// Service contract for Activity operations
    /// </summary>
    public interface IActivityService
    {
        Task<PagedResultDto<ActivityDto>> GetAllActivitiesAsync(string userId, ActivitiesRequestParams query);
        Task<ActivityDto> GetActivityByIdAsync(int id, string userId);
        Task<ActivityDetailDto> GetActivityDetailByIdAsync(int id);
        Task<ActivityDto> CreateActivityAsync(CreateActivityDto dto);
        Task UpdateActivityAsync(int id, UpdateActivityDto dto);
        Task PatchActivityAsync(int id, PatchActivityDto dto);
        Task DeleteActivityAsync(int id);
    }
}
