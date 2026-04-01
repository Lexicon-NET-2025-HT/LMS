namespace LMS.Shared.DTOs.Submission
{
    /// <summary>
    /// Data transfer object for updating an existing submission.
    /// Used in PUT  /api/submissions/{id} requests.
    /// Students can only update their own submissions before the deadline.
    /// </summary>
    public record UpdateSubmissionDto
    {
        public int ActivityId { get; set; }
        public string Body { get; set; } = string.Empty;
        public int? DocumentId { get; set; }
    }
}