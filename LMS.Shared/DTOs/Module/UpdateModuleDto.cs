using System;

namespace LMS.Shared.DTOs.Module
{
    /// <summary>
    /// Data transfer object for updating an existing module.
    /// Used in PUT /api/modules/{id} requests.
    /// All properties are nullable to support partial updates.
    /// </summary>
    public record UpdateModuleDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}