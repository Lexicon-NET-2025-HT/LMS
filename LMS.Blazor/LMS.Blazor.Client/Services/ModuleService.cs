using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Module;

namespace LMS.Blazor.Client.Services;

public class ModuleService : IModuleService
{
    private readonly IApiService _apiService;
    private const string Base = "api/modules";

    public ModuleService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public Task<PagedResultDto<ModuleDto>?> GetAllModulesAsync(
        int page = 1,
        int pageSize = 10,
        int? courseId = null,
        CancellationToken ct = default)
    {
        var query = $"{Base}?page={page}&pageSize={pageSize}";

        if (courseId.HasValue)
            query += $"&courseId={courseId.Value}";

        return _apiService.GetAsync<PagedResultDto<ModuleDto>>(query, ct);
    }

    public Task<ModuleDto?> GetModuleByIdAsync(int id, CancellationToken ct = default)
        => _apiService.GetAsync<ModuleDto>($"{Base}/{id}", ct);

    public Task<ModuleDetailDto?> GetModuleDetailByIdAsync(int id, CancellationToken ct = default)
        => _apiService.GetAsync<ModuleDetailDto>($"{Base}/{id}/detail", ct);

    public Task<ModuleDto?> CreateModuleAsync(CreateModuleDto dto, CancellationToken ct = default)
        => _apiService.PostAsync<ModuleDto>(Base, dto, ct);

    public Task DeleteModuleAsync(int id, CancellationToken ct = default)
        => _apiService.DeleteAsync($"{Base}/{id}", ct);
}