namespace LMS.Shared.DTOs.CourseDtos;

public class UpdateCourseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
}