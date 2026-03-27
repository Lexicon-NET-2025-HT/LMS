namespace LMS.Shared.DTOs.User;

public record CreateUserDto
{
    // public required string Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string Role { get; set; } = "Student";
    public int? CourseId { get; set; }
    // public string? CourseName { get; set; }
}
