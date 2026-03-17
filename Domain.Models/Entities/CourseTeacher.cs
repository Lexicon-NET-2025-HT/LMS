namespace Domain.Models.Entities;

public class CourseTeacher
{
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public Guid TeacherId { get; set; }
    public ApplicationUser Teacher { get; set; } = null!;
}
