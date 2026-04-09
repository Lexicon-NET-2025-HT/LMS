using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Infractructure.Extensions;
using LMS.Services.Access;
using LMS.Shared.DTOs.Activity;
using LMS.Shared.DTOs.Common;
using LMS.Shared.Request;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;

namespace LMS.Services
{
    /// <summary>
    /// Activity service implementation
    /// </summary>
    public class ActivityService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILmsAccessService lmsAccessService,
        IUserAccessContextFactory userAccessContextFactory,
        UserManager<ApplicationUser> userManager) : IActivityService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILmsAccessService _lmsAccessService = lmsAccessService;
        private readonly IUserAccessContextFactory _userAccessContextFactory = userAccessContextFactory;

        public async Task<PagedResultDto<ActivityDto>> GetAllActivitiesAsync(string userId, ActivitiesRequestParams queryDto)
        {
            var access = await _userAccessContextFactory.CreateAsync(userId);

            if (queryDto.ModuleId != null)
            {
                var module = await _unitOfWork.Modules.GetModuleAsync(queryDto.ModuleId.Value) ??
                    throw new NotFoundException($"Module by id: '{queryDto.ModuleId.Value}', does not exist");
                await lmsAccessService.EnsureCanAccessModuleAsync(userId, module);
            }

            var query = lmsAccessService.ApplyActivityAccessFilter(
                _unitOfWork.Activities.BuildQuery(queryDto.ModuleId),
                access);

            var (activities, totalCount) = await query
                .OrderBy(a => a.StartTime)
                .PagedResult(queryDto);

            return new PagedResultDto<ActivityDto>
            {
                Items = _mapper.Map<List<ActivityDto>>(activities),
                TotalCount = totalCount,
                PageNumber = queryDto.Page,
                PageSize = queryDto.PageSize
            };
        }

        public async Task<ActivityDto> GetActivityByIdAsync(int id, string userId)
        {
            var activity = await _unitOfWork.Activities.GetActivityWithRelationsAsync(id) ??
                throw new NotFoundException($"Activity by id: '{id}', does not exist");

            await lmsAccessService.EnsureCanAccessActivityAsync(userId, activity);

            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<ActivityDetailDto> GetActivityDetailByIdAsync(int id)
        {
            var activity = await _unitOfWork.Activities.GetActivityWithDetailAsync(id) ??
                throw new NotFoundException($"Activity by id: '{id}', does not exist");

            Console.WriteLine("TESTTTTT");
            Console.WriteLine(activity.GetType().Name);

            var activityDetailDto = _mapper.Map<ActivityDetailDto>(activity);

            return activityDetailDto;
        }

        public async Task<ActivityDto> CreateActivityAsync(CreateActivityDto dto)
        {
            var module = await _unitOfWork.Modules.FindByIdAsync(dto.ModuleId) ??
                throw new NotFoundException($"Module by id: '{dto.ModuleId}', does not exist");

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
                throw new NotFoundException($"Activity by id: '{id}', does not exist");

            _mapper.Map(dto, activity);

            _unitOfWork.Activities.Update(activity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task PatchActivityAsync(int id, PatchActivityDto dto)
        {
            var activity = await _unitOfWork.Activities.FindByIdAsync(id) ??
                throw new NotFoundException($"Activity by id: '{id}', does not exist");

            _mapper.Map(dto, activity);

            _unitOfWork.Activities.Update(activity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteActivityAsync(int id)
        {
            var activity = await _unitOfWork.Activities.FindByIdAsync(id) ??
                throw new NotFoundException($"Activity with id: '{id}' does not exist");

            _unitOfWork.Activities.Delete(activity);
            await _unitOfWork.CompleteAsync();
        }
    }
}
