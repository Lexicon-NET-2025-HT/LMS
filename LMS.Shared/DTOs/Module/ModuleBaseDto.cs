namespace LMS.Shared.DTOs.Module;

/// <summary>
/// Data transfer object for creating a new module.
/// Used in POST /api/modules requests.
/// </summary>
public record ModuleBaseDto
{
    public required int CourseId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; } = string.Empty;
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }

}