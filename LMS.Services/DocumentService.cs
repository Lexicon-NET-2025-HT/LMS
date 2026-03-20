using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Document;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;

namespace LMS.Services
{
    public class DocumentService(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager) : IDocumentService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UploadedByUserId) ?? 
                throw new Exception($"User by id {dto.UploadedByUserId} does not exist");

            var document = await _unitOfWork.Documents.CreateAsync(
                new Document
                {
                    FileName = dto.FileName,
                    DisplayName = dto.DisplayName,
                    Description = dto.Description,
                    UploadedAt = DateTime.Now,
                    UploadedByUserId = dto.UploadedByUserId,
                    UploadedByUser = user,
                    Course = dto.CourseId.HasValue ? await _unitOfWork.Courses.FindByIdAsync(dto.CourseId) : null,
                    CourseId = dto.CourseId,
                    Module = dto.ModuleId.HasValue ? await _unitOfWork.Modules.FindByIdAsync(dto.ModuleId) : null,
                    ModuleId = dto.ModuleId,
                    Activity = dto.ActivityId.HasValue ? await _unitOfWork.Activities.FindByIdAsync(dto.ActivityId) : null,
                    ActivityId = dto.ActivityId
                }
            );

            await _unitOfWork.CompleteAsync();

            return new DocumentDto
            {
                Id = document.Id,
                FileName = document.FileName,
                DisplayName = document.DisplayName,
                Description = document.Description,
                UploadedAt = document.UploadedAt,
                UploadedByUserId = document.UploadedByUserId,
                UploadedByUserName = document.UploadedByUser.UserName ?? string.Empty,
                CourseId = document.CourseId,
                ModuleId = document.ModuleId,
                ActivityId = document.ActivityId,
                Scope = document.CourseId.HasValue ? "Course" : document.ModuleId.HasValue ? "Module" : "Activity"
            };
        }

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
