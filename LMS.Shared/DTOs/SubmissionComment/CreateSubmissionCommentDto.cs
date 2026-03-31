namespace LMS.Shared.DTOs.SubmissionComment;

public class CreateSubmissionCommentDto
{
    public int SubmissionId { get; set; }
    public string Text { get; set; } = string.Empty;

}