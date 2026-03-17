namespace Domain.Models.Entities;

public class Document
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public string UploadedByUserId { get; set; } = string.Empty;
    public int? CourseId { get; set; }
    public Course? Course { get; set; }
    public int? ModuleId { get; set; }
    public Module? Module { get; set; }
    public int? ActivityId { get; set; }
    public Activity? Activity { get; set; }
}
