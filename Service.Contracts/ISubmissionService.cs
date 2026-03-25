using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Submission;

namespace Service.Contracts
{
    /// <summary>
    /// Service contract for Submission operations
    /// </summary>
    public interface ISubmissionService
    {
        Task<PagedResultDto<SubmissionDto>> GetAllSubmissionsAsync(int page, int pageSize, int? activityId = null, string? studentId = null);
        Task<SubmissionDto?> GetSubmissionByIdAsync(int id);
        Task<SubmissionDto> CreateSubmissionAsync(string userId, CreateSubmissionDto dto);
        Task UpdateSubmissionAsync(int id, UpdateSubmissionDto dto);
        Task DeleteSubmissionAsync(int id);
        Task SubmitFeedbackAsync(int id, SubmitFeedbackDto dto);
    }
}
