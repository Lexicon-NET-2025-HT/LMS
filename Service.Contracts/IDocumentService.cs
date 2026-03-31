using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Document;

namespace Service.Contracts
{
    /// <summary>
    /// Service contract for Document operations
    /// </summary>
    public interface IDocumentService
    {
        Task<PagedResultDto<DocumentDto>> GetAllDocumentsAsync(string userId, int page, int pageSize, int? courseId = null);
        Task<DocumentDto?> GetDocumentByIdAsync(int id, string userId);
        Task<DocumentDto> CreateDocumentAsync(string userId, CreateDocumentDto dto);
        Task UpdateDocumentAsync(int id, UpdateDocumentDto dto);
        Task DeleteDocumentAsync(int id, string userId);
    }
}
