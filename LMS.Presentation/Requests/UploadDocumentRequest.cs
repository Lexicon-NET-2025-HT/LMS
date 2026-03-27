using Microsoft.AspNetCore.Http;

namespace LMS.Presentation.Requests;

public class UploadDocumentRequest
{
    public IFormFile File { get; init; } = null!;
    public required string Description { get; init; }
    public required Stream FileStream { get; init; } = null!;
    public int? CourseId { get; init; } = null;
    public int? ModuleId { get; init; } = null;
    public int? ActivityId { get; init; } = null;
    public int? SubmissionId { get; init; } = null;
}