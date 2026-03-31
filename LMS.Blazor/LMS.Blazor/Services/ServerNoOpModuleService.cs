using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Module;

namespace LMS.Blazor.Services;

public class ServerNoOpModuleService : IModuleService
{
    public Task<PagedResultDto<ModuleDto>?> GetAllModulesAsync(int page = 1, int pageSize = 10, int? courseId = null, CancellationToken ct = default)
        => Task.FromResult<PagedResultDto<ModuleDto>?>(null);

    public Task<ModuleDto?> GetModuleByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult<ModuleDto?>(null);

    public Task<ModuleDetailDto?> GetModuleDetailByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult<ModuleDetailDto?>(null);

    public Task<ModuleDto?> CreateModuleAsync(CreateModuleDto dto, CancellationToken ct = default)
        => Task.FromResult<ModuleDto?>(null);

    public Task DeleteModuleAsync(int id, CancellationToken ct = default)
        => Task.CompletedTask;
}
