using AutoMapper;
using Domain.Contracts.Repositories;
using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Module;
using LMS.Shared.DTOs.Submission;
using Service.Contracts;

namespace LMS.Services
{
    /// <summary>
    /// Submission service implementation
    /// </summary>
    public class SubmissionService : ISubmissionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public SubmissionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<PagedResultDto<SubmissionDto>> GetAllSubmissionsAsync(int page, int pageSize, int? activityId = null, string? studentId = null)
        {
            var (submissions, totalCount) = await unitOfWork.Submissions.GetAllSubmissionsAsync(page, pageSize, activityId);

            return new PagedResultDto<ModuleDto>
            {
                Items = mapper.Map<List<SubmissionDto>>(submissions),
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<SubmissionDto?> GetSubmissionByIdAsync(int id)
        {
            throw new NotImplementedException();
            // TODO: Replace with real database query
            //return await Task.FromResult(new SubmissionDto
            //{
            //    Id = id,
            //    StudentId = "student-1",
            //    StudentName = "John Doe",
            //    ActivityId = 1,
            //    ActivityName = "Variables and Data Types",
            //    Body = "My submission for this assignment...",
            //    DocumentId = null,
            //    Document = null,
            //    SubmittedAt = DateTime.Now,
            //    IsLate = false,
            //    FeedbackText = null,
            //    FeedbackGivenAt = null
            //});
        }

        public async Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionDto dto)
        {
            throw new NotImplementedException();
            // TODO: Create entity and save to database
            //return await Task.FromResult(new SubmissionDto
            //{
            //    Id = 1,
            //    StudentId = dto.StudentId,
            //    StudentName = "Student Name",
            //    ActivityId = dto.ActivityId,
            //    ActivityName = "Activity Name",
            //    Body = dto.Body,
            //    DocumentId = dto.DocumentId,
            //    Document = null,
            //    SubmittedAt = DateTime.Now,
            //    IsLate = false,
            //    FeedbackText = null,
            //    FeedbackGivenAt = null
            //});
        }

        public async Task UpdateSubmissionAsync(int id, UpdateSubmissionDto dto)
        {
            // TODO: Update entity in database
            await Task.CompletedTask;
        }

        public async Task DeleteSubmissionAsync(int id)
        {
            // TODO: Delete entity from database
            await Task.CompletedTask;
        }

        public async Task SubmitFeedbackAsync(int id, SubmitFeedbackDto dto)
        {
            // TODO: Update submission with feedback
            await Task.CompletedTask;
        }
    }
}
