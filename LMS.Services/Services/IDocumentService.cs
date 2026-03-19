using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Document;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Services.Services
{
    /// <summary>
    /// Service contract for Document operations
    /// </summary>
    public interface IDocumentService
    {
        Task<PagedResultDto<DocumentDto>> GetAllDocumentsAsync(int page, int pageSize);
        Task<DocumentDto?> GetDocumentByIdAsync(int id);
        Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto dto);
        Task UpdateDocumentAsync(int id, UpdateDocumentDto dto);
        Task DeleteDocumentAsync(int id);
    }
}
