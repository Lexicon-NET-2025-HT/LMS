using System;

namespace LMS.Shared.DTOs.Module
{
    /// <summary>
    /// Data transfer object for creating a new module.
    /// Used in POST /api/modules requests.
    /// </summary>
    public record CreateModuleDto
    {
        public required int CourseId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}