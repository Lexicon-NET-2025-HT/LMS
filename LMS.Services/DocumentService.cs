using LMS.Services;
using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Document;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Services
{
    public class DocumentService : IDocumentService
    {
        public async Task<PagedResultDto<DocumentDto>> GetAllDocumentsAsync(int page, int pageSize)
        {
            // TODO: Replace with real database query
            var mockDocuments = new List<DocumentDto>
        {
            new DocumentDto
            {
                Id = 1,
                FileName = "lecture-notes.pdf",
                DisplayName = "Lecture Notes",
                Description = "Notes for C# variables",
                UploadedAt = DateTime.Now,
                UploadedByUserId = "teacher-1",
                UploadedByUserName = "Dr. Smith",
                ActivityId = 1,
                Scope = "Activity"
            }
        };

            return await Task.FromResult(new PagedResultDto<DocumentDto>
            {
                Items = mockDocuments,
                TotalCount = mockDocuments.Count,
                PageNumber = page,
                PageSize = pageSize
            });
        }

        public async Task<DocumentDto?> GetDocumentByIdAsync(int id)
        {
            // TODO: Replace with real database query
            return await Task.FromResult(new DocumentDto
            {
                Id = id,
                FileName = "lecture-notes.pdf",
                DisplayName = "Lecture Notes",
                Description = "Notes for C# variables",
                UploadedAt = DateTime.Now,
                UploadedByUserId = "teacher-1",
                UploadedByUserName = "Dr. Smith",
                ActivityId = 1,
                Scope = "Activity"
            });
        }

        public async Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto dto)
        {
            // TODO: Create entity and save to database
            return await Task.FromResult(new DocumentDto
            {
                Id = 1,
                FileName = dto.FileName,
                DisplayName = dto.DisplayName,
                Description = dto.Description,
                UploadedAt = DateTime.Now,
                UploadedByUserId = dto.UploadedByUserId,
                UploadedByUserName = "User Name",
                CourseId = dto.CourseId,
                ModuleId = dto.ModuleId,
                ActivityId = dto.ActivityId,
                Scope = dto.ActivityId.HasValue ? "Activity" : dto.ModuleId.HasValue ? "Module" : "Course"
            });
        }

        public async Task UpdateDocumentAsync(int id, UpdateDocumentDto dto)
        {
            // TODO: Update entity in database
            await Task.CompletedTask;
        }

        public async Task DeleteDocumentAsync(int id)
        {
            // TODO: Delete entity from database
            await Task.CompletedTask;
        }
    }
}
