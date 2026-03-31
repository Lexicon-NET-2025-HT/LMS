using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

public interface IDocumentRepository : IRepositoryBase<Document>, IInternalRepositoryBase<Document>
{
    Task<(IEnumerable<Document> documents, int totalCount)> GetAllDocumentsAsync(int page, int pageSize, int? courseId);
    Task<Document?> GetDocumentAsync(int id, bool trackChanges = false);
    Task<Document?> GetDocumentWithOwnershipChainAsync(int id, bool trackChanges = false);
}