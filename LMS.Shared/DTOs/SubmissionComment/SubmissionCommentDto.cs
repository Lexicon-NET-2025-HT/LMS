namespace LMS.Shared.DTOs.SubmissionComment;

public class SubmissionCommentDto
{
    public int Id { get; set; }
    public int SubmissionId { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

}