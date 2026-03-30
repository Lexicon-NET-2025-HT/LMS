namespace LMS.Shared.DTOs.Document
{
    /// <summary>
    /// Represents a document attached to a course, module, or activity.
    /// Documents can be associated with one of three scopes: Course, Module, or Activity.
    /// </summary>
    public record DocumentDto
    {
        public required int Id { get; set; }
        /// <summary>
        /// Original filename as uploaded by the user. This is not the name used for storage, which is typically a unique identifier to avoid conflicts.
        /// </summary>
        public required string FileName { get; set; }
        /// <summary>
        /// Publicly accessible URL or path to access the stored document.
        /// </summary>
        public required string Path { get; set; }
        public long FileSize { get; set; }
        public required string DisplayName { get; set; }
        public required string Description { get; set; }

        /// <summary>
        /// Date and time when the document was uploaded.
        /// </summary>
        public required DateTime UploadedAt { get; set; }

        /// <summary>
        /// User ID of the person who uploaded the document.
        /// References APPLICATIONUSER.Id.
        /// </summary>
        public required string UploadedByUserId { get; set; }

        /// <summary>
        /// Name of the user who uploaded the document.
        /// Populated by joining with APPLICATIONUSER table.
        /// </summary>
        public required string UploadedByUserName { get; set; }
        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public int? ActivityId { get; set; }
        public int? SubmissionId { get; set; }
        public required string Scope { get; set; }
    }
}