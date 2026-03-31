using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Infractructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LMS.Infractructure.Repositories;

public class DocumentRepository(ApplicationDbContext context) : RepositoryBase<Document>(context), IDocumentRepository
{
    public async Task<(IEnumerable<Document> documents, int totalCount)> GetAllDocumentsAsync(int page, int pageSize, int? courseId)
    {
        IQueryable<Document> query = FindAll(trackChanges: false)
            .Include(d => d.UploadedByUser)
            .Include(d => d.Course)
            .Include(d => d.Module)
            .Include(d => d.Activity)
            .Include(d => d.Submission);

        if (courseId.HasValue)
        {
            query = query.Where(d => d.CourseId == courseId.Value);
        }

        var (documents, totalCount) = await query
            .OrderByDescending(d => d.UploadedAt)
            .PagedResult(page, pageSize);

        return (documents, totalCount);
    }
    public async Task<Document?> GetDocumentAsync(int id, bool trackChanges = false)
    {
        var document = FindByCondition(d => d.Id == id, trackChanges)
            .Include(d => d.UploadedByUser)
            .Include(d => d.Course)
            .Include(d => d.Module)
            .Include(d => d.Activity)
            .Include(d => d.Submission);

        return await document.FirstOrDefaultAsync();
    }

    public async Task<Document?> GetDocumentWithOwnershipChainAsync(int id, bool trackChanges = false)
    {
        return await FindByCondition(d => d.Id == id, trackChanges)
            .Include(d => d.Course)
            .Include(d => d.Module)
                .ThenInclude(m => m.Course)
                    .ThenInclude(c => c.CourseTeachers)
            .Include(d => d.Activity)
                .ThenInclude(a => a.Module)
                    .ThenInclude(m => m.Course)
                        .ThenInclude(c => c.CourseTeachers)
            .Include(d => d.Submission)
                .ThenInclude(s => s.Activity)
                    .ThenInclude(a => a.Module)
                        .ThenInclude(m => m.Course)
                            .ThenInclude(c => c.CourseTeachers)
            .FirstOrDefaultAsync();
    }
}