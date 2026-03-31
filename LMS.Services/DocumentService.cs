using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Contracts.Storage;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.Common;
using LMS.Shared.DTOs.Document;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;

namespace LMS.Services
{
    public class DocumentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileStorage fileStorage,
        UserManager<ApplicationUser> userManager) : IDocumentService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IFileStorage _fileStorage = fileStorage;
        public async Task<DocumentDto> CreateDocumentAsync(string userId, CreateDocumentDto dto)
        {
            if (dto.File == null)
            {
                throw new BadRequestException("File is missing.");
            }

            if (dto.CourseId is null &&
                dto.ModuleId is null &&
                dto.ActivityId is null &&
                dto.SubmissionId is null)
            {
                throw new BadRequestException("Document must belong to a course, module, activity or submission.");
            }

            //DisplayName = request.File.FileName,
            //ContentType = request.File.ContentType,
            //FileSize = request.File.Length
            var user = await _userManager.FindByIdAsync(userId) ??
                throw new UnauthorizedAccessException($"User by id {userId} does not exist");

            var savedFileResult = await _fileStorage.SaveAsync(dto.File);

            var document = _mapper.Map<Document>(dto);

            document.UploadedByUser = user;
            document.UploadedAt = DateTime.UtcNow;
            document.FileSize = savedFileResult.FileSize;
            document.StoredFileName = savedFileResult.FileName;

            _unitOfWork.Documents.Create(document);
            await _unitOfWork.CompleteAsync();

            var createdDocument = await _unitOfWork.Documents.GetDocumentAsync(document.Id);

            return _mapper.Map<DocumentDto>(document);
        }
        public async Task UpdateDocumentAsync(int id, UpdateDocumentDto dto)
        {
            var document = await _unitOfWork.Documents.FindByIdAsync(id) ??
                throw new NotFoundException($"Document with id: '{id}' does not exist");

            document.DisplayName = dto.DisplayName ?? string.Empty;
            document.Description = dto.Description ?? string.Empty;

            _unitOfWork.Documents.Update(document);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteDocumentAsync(int id, string userId)
        {
            var document = await unitOfWork.Documents.GetDocumentWithOwnershipChainAsync(id)
                ?? throw new NotFoundException($"Document with id: '{id}' does not exist");

            await ThrowIfNoAccess(userId, document);

            // TODO: option to soft delete instead of hard delete, add IsDeleted property to Document entity and filter it out in queries

            // remove the file from storage if it exists
            if (!string.IsNullOrWhiteSpace(document.StoredFileName))
            {
                var success = await _fileStorage.DeleteAsync(document.StoredFileName);
                if (!success)
                {
                    // log the failure to delete the file, but do not prevent the document record from being deleted
                    // consider implementing a retry mechanism or marking the document for cleanup if file deletion fails
                }
            }

            _unitOfWork.Documents.Delete(document);
            await _unitOfWork.CompleteAsync();
        }

        private async Task ThrowIfNoAccess(string userId, Document document)
        {
            if (document.UploadedByUserId != userId)
            {
                var course = ResolveCourse(document);

                if (course == null)
                {
                    // pretend the file doesn't exist to prevent data leakage
                    throw new NotFoundException("Document is not linked to a course.");
                }

                if (!IsTeacherForCourse(userId, course))
                {
                    // pretend the file doesn't exist to prevent data leakage
                    throw new NotFoundException($"Document with id {document.Id} does not exist");
                }

            }
        }

        public async Task<PagedResultDto<DocumentDto>> GetAllDocumentsAsync(string userId, int page, int pageSize, int? courseId = null)
        {
            (var documents, int totalCount) = await _unitOfWork.Documents.GetAllDocumentsAsync(page, pageSize, courseId);

            var dtos = _mapper.Map<List<DocumentDto>>(documents);

            return new PagedResultDto<DocumentDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<DocumentDto?> GetDocumentByIdAsync(int id, string userId)
        {
            var document = await _unitOfWork.Documents.GetDocumentAsync(id, false)
                ?? throw new NotFoundException($"Document with id {id} does not exist");

            await ThrowIfNoAccess(userId, document);

            return _mapper.Map<DocumentDto>(document);
        }

        private static Course? ResolveCourse(Document d)
        {
            return d.Course
                ?? d.Module?.Course
                ?? d.Activity?.Module?.Course
                ?? d.Submission?.Activity?.Module?.Course;
        }

        private static bool IsTeacherForCourse(string userId, Course course)
        {
            return course.CourseTeachers.Any(t => t.TeacherId == userId);
        }

    }
}