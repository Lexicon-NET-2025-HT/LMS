using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Module;

namespace LMS.Blazor.Client.Services;

public interface IModuleService
{
    Task<PagedResultDto<ModuleDto>?> GetAllModulesAsync(int page = 1, int pageSize = 10, int? courseId = null, CancellationToken ct = default);
    Task<ModuleDto?> GetModuleByIdAsync(int id, CancellationToken ct = default);
    Task<ModuleDetailDto?> GetModuleDetailByIdAsync(int id, CancellationToken ct = default);
    Task<ModuleDto?> CreateModuleAsync(CreateModuleDto dto, CancellationToken ct = default);
    Task<bool> DeleteModuleAsync(int id, CancellationToken ct = default);
}