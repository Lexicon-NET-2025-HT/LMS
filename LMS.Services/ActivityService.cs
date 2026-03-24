using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Enums;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.Activity;
using LMS.Shared.DTOs.Common;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;

namespace LMS.Services
{
    /// <summary>
    /// Activity service implementation - TODO: Replace with real database operations
    /// </summary>
    public class ActivityService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<ApplicationUser> userManager) : IActivityService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PagedResultDto<ActivityDto>> GetAllActivitiesAsync(int page, int pageSize, int? moduleId = null)
        {
            // TODO: Replace with real database query
            var mockActivities = new List<ActivityDto>
        {
            new ActivityDto
            {
                Id = 1,
                ModuleId = moduleId ?? 1,
                ModuleName = "Introduction to C#",
                Name = "Variables and Data Types",
                Description = "Learn about variables",
                Type = ActivityType.Lecture,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2),
                DocumentCount = 3,
                SubmissionCount = 0
            }
        };

            return await Task.FromResult(new PagedResultDto<ActivityDto>
            {
                Items = mockActivities,
                TotalCount = mockActivities.Count,
                PageNumber = page,
                PageSize = pageSize
            });
        }

        public async Task<ActivityDto?> GetActivityByIdAsync(int id)
        {
            var activity = await _unitOfWork.Activities.FindByIdAsync(id) ??
                throw new Exception($"Activity by id: '{id}', does not exist");

            var activityDto = _mapper.Map<ActivityDto>(activity);

            return activityDto;
        }

        public async Task<ActivityDetailDto?> GetActivityDetailByIdAsync(int id)
        {
            // TODO: Replace with real database query
            return await Task.FromResult(new ActivityDetailDto
            {
                Id = id,
                ModuleId = 1,
                ModuleName = "Introduction to C#",
                Name = "Variables and Data Types",
                Description = "Learn about variables",
                Type = ActivityType.Lecture,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2),
                DocumentCount = 3,
                SubmissionCount = 0,
                Documents = new(),
                Submissions = new()
            });
        }

        public async Task<ActivityDto> CreateActivityAsync(CreateActivityDto dto)
        {
            var module = await _unitOfWork.Modules.FindByIdAsync(dto.ModuleId) ??
                throw new Exception($"Module by id: '{dto.ModuleId}', does not exist");

            var activity = _mapper.Map<Activity>(dto);

            activity.Module = module;

            _unitOfWork.Activities.Create(activity);
            await _unitOfWork.CompleteAsync();

            var activityDto = _mapper.Map<ActivityDto>(activity);

            return activityDto;
        }

        public async Task UpdateActivityAsync(int id, UpdateActivityDto dto)
        {
            var activity = await _unitOfWork.Activities.FindByIdAsync(id) ??
                throw new Exception($"Activity by id: '{id}', does not exist");

            activity.Name = dto.Name ?? activity.Name;
            activity.Description = dto.Description ?? activity.Description;
            activity.Type = dto.Type ?? activity.Type;
            activity.StartTime = dto.StartTime;
            activity.EndTime = dto.EndTime ?? activity.EndTime;

            _unitOfWork.Activities.Update(activity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteActivityAsync(int id)
        {
            // TODO: Delete entity from database
            await Task.CompletedTask;
        }
    }
}
