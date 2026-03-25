namespace Domain.Models.Entities;

public class SubmissionComment : EntityBase
{
    public int SubmissionId { get; set; }
    public Submission Submission { get; set; } = null!;

    public string AuthorId { get; set; } = string.Empty;
    public ApplicationUser Author { get; set; } = null!;

    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

}