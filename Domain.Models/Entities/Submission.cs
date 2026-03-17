namespace Domain.Models.Entities;

public class Submission
{
    public int Id { get; set; }
    public Guid StudentId { get; set; }
    public ApplicationUser Student { get; set; } = null!;
    public int ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;
    public string? Body { get; set; }
    public int? DocumentId { get; set; }
    public DateTime SubmittedAt { get; set; }
    public bool IsLate { get; set; }
    public string FeedbackText { get; set; } = string.Empty;
    public DateTime FeedbackGivenAt { get; set; }

}
