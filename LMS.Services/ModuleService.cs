using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Module;
using Service.Contracts;

namespace LMS.Services
{
    /// <summary>
    /// Module service implementation - TODO: Replace with real database operations
    /// </summary>
    public class ModuleService : IModuleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ModuleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<PagedResultDto<ModuleDto>> GetAllModulesAsync(int page, int pageSize, int? courseId = null)
        {
            var (modules, totalCount) = await unitOfWork.Modules.GetAllModulesAsync(page, pageSize, courseId);

            return new PagedResultDto<ModuleDto>
            {
                Items = mapper.Map<List<ModuleDto>>(modules),
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<ModuleDto?> GetModuleByIdAsync(int id)
        {
            var module = await unitOfWork.Modules.GetModuleAsync(id);
            return mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDetailDto?> GetModuleDetailByIdAsync(int id)
        {
            var module = await unitOfWork.Modules.GetModuleAsync(id);
            return mapper.Map<ModuleDetailDto>(module);
        }

        public async Task<ModuleDto> CreateModuleAsync(CreateModuleDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (!await unitOfWork.Courses.ExistsAsync(dto.CourseId))
            {
                throw new KeyNotFoundException($"Course with id {dto.CourseId} was not found.");
            }


            var module = mapper.Map<Module>(dto);
            unitOfWork.Modules.Create(module);
            await unitOfWork.CompleteAsync();

            // refetch new module to verify it exists
            var createdModule = await unitOfWork.Modules.GetModuleAsync(module.Id)
                ?? throw new InvalidOperationException($"Module with id {module.Id} could not be loaded after creation.");

            return mapper.Map<ModuleDto>(createdModule);

        }

        public async Task UpdateModuleAsync(int id, UpdateModuleDto dto)
        {
            // TODO: Update entity in database
            await Task.CompletedTask;
        }

        public async Task DeleteModuleAsync(int id)
        {
            // TODO: Delete entity from database
            await Task.CompletedTask;
        }
    }
}
