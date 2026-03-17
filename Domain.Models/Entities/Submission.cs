namespace Domain.Models.Entities;

public class Submission
{
    public int Id { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public int ActivityId { get; set; }
    public string Body { get; set; } = string.Empty;
    public int? DocumentId { get; set; }
    public DateTime SubmittedAt { get; set; }
    public bool IsLate { get; set; }
    public string FeedbackText { get; set; } = string.Empty;
    public DateTime FeedbackGivenAt { get; set; }
}
