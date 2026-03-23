using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Document;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Service.Contracts;

namespace LMS.Services
{
    public class DocumentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<ApplicationUser> userManager) : IDocumentService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UploadedByUserId) ??
                throw new Exception($"User by id {dto.UploadedByUserId} does not exist");

            var document = _mapper.Map<Document>(dto);

            document.UploadedByUser = user;
            document.Course = document.CourseId.HasValue ? await _unitOfWork.Courses.FindByIdAsync(dto.CourseId) : null;
            document.Module = document.ModuleId.HasValue ? await _unitOfWork.Modules.FindByIdAsync(dto.ModuleId) : null;
            document.Activity = document.ActivityId.HasValue ? await _unitOfWork.Activities.FindByIdAsync(dto.CourseId) : null;

            Console.WriteLine("THIS IS MY WRITE LINE!!!");
            Console.WriteLine(document.UploadedByUser.UserName);

            _unitOfWork.Documents.Create(document);
            await _unitOfWork.CompleteAsync();

            var documentDto = _mapper.Map<DocumentDto>(document);

            return documentDto;
        }
        public async Task UpdateDocumentAsync(int id, UpdateDocumentDto dto)
        {
            var document = await _unitOfWork.Documents.FindByIdAsync(id) ??
                throw new Exception($"Document with id: '{id}' does not exist");

            document.DisplayName = dto.DisplayName ?? string.Empty;
            document.Description = dto.Description ?? string.Empty;

            _unitOfWork.Documents.Update(document);
            await _unitOfWork.CompleteAsync();
        }
        public async Task DeleteDocumentAsync(int id)
        {
            var document = await _unitOfWork.Documents.FindByIdAsync(id) ??
                throw new Exception($"Document with id: '{id}' does not exist");

            _unitOfWork.Documents.Delete(document);
            await _unitOfWork.CompleteAsync();
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
            var document = await _unitOfWork.Documents.FindByIdAsync(id) ??
                throw new NotFoundException($"Document with id: '{id}' does not exist");

            var documentDto = _mapper.Map<DocumentDto>(document);

            return documentDto;
        }
    }
}