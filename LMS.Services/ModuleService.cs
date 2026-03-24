using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Module;
using Service.Contracts;

namespace LMS.Services
{
    /// <summary>
    /// Module service implementation
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

            await ThrowIfNotValidModule(dto.CourseId, 0, dto.StartDate, dto.EndDate, dto.Name);

            var module = mapper.Map<Module>(dto);
            unitOfWork.Modules.Create(module);
            await unitOfWork.CompleteAsync();

            // refetch new module to verify it exists
            var createdModule = await unitOfWork.Modules.GetModuleAsync(module.Id)
                ?? throw new InvalidOperationException($"Module with id {module.Id} could not be loaded after creation.");

            return mapper.Map<ModuleDto>(createdModule);

        }

        private async Task ThrowIfNotValidModule(Module module)
        {
            await ThrowIfNotValidModule(module.CourseId, module.Id, module.StartDate, module.EndDate, module.Name);
        }

        private async Task ThrowIfNotValidModule(int courseId,
                                                 int moduleId,
                                                 DateTime moduleStartDate,
                                                 DateTime moduleEndDate,
                                                 string name)
        {
            var course = await unitOfWork.Courses.FindByIdAsync(courseId);
            if (course is null)
            {
                throw new KeyNotFoundException($"Course with id {courseId} was not found.");
            }
            if (course.StartDate > moduleStartDate)
            {
                throw new BadRequestException("Module start date cannot be earlier than course start date.");
            }
            if (moduleStartDate >= moduleEndDate)
            {
                throw new BadRequestException("End date must be greater than start date.");
            }

            var existingModule = await unitOfWork.Modules.GetModuleByNameAsync(courseId, name);
            if (existingModule is not null && existingModule.Id != moduleId)
            {
                throw new BadRequestException("Module name must be unique within course");
            }
            // TODO: add validation for...
            // 1 not overlapping modules
            // 2 setting start and end times to reasonable values (08:00 - 17:00?)
            // 3 checking for weekends? assignment tasks within a module could be set on sundays, but maybe a module never starts on a weekend

        }

        public async Task UpdateModuleAsync(int id, UpdateModuleDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var module = await unitOfWork.Modules.GetModuleAsync(id, trackChanges: true);
            if (module is null)
            {
                throw new KeyNotFoundException($"Module with id {id} was not found.");
            }

            module.Name = dto.Name;
            module.Description = dto.Description;
            module.StartDate = module.StartDate;
            module.EndDate = module.EndDate;

            await ThrowIfNotValidModule(module);

            await unitOfWork.CompleteAsync();
        }

        public async Task PatchModuleAsync(int id, PatchModuleDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var module = await unitOfWork.Modules.GetModuleAsync(id, trackChanges: true);
            if (module is null)
            {
                throw new KeyNotFoundException($"Module with id {id} was not found.");
            }

            module.Name = string.IsNullOrEmpty(dto.Name) ? module.Name : dto.Name;
            module.Description = string.IsNullOrEmpty(dto.Description) ? module.Description : dto.Description;
            module.StartDate = dto.StartDate ?? module.StartDate;
            module.EndDate = dto.EndDate ?? module.EndDate;

            await ThrowIfNotValidModule(module.CourseId, module.Id, module.StartDate, module.EndDate, module.Name);

            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteModuleAsync(int id)
        {
            var module = await unitOfWork.Modules.GetModuleAsync(id, trackChanges: true);
            if (module is null)
            {
                throw new KeyNotFoundException($"Module with id {id} was not found.");
            }

            unitOfWork.Modules.Delete(module);
            await unitOfWork.CompleteAsync();
        }
    }
}
