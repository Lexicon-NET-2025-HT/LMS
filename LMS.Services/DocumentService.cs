using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Contracts.Storage;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Infractructure.Extensions;
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
                throw new ArgumentException("File is missing.");
            }

            if (dto.CourseId is null &&
                dto.ModuleId is null &&
                dto.ActivityId is null &&
                dto.SubmissionId is null)
            {
                throw new ArgumentException("Document must belong to a course, module, activity or submission.");
            }

            //DisplayName = request.File.FileName,
            //ContentType = request.File.ContentType,
            //FileSize = request.File.Length
            var user = await _userManager.FindByIdAsync(userId) ??
                throw new Exception($"User by id {userId} does not exist");

            var savedFileResult = await _fileStorage.SaveAsync(dto.File);

            var document = _mapper.Map<Document>(dto);

            document.UploadedByUser = user;
            document.UploadedAt = DateTime.UtcNow;
            document.FileSize = savedFileResult.FileSize;
            document.StoredFileName = savedFileResult.FileName;

            _unitOfWork.Documents.Create(document);
            await _unitOfWork.CompleteAsync();

            var createdDocument = await _unitOfWork.Documents.FindByIdAsync(document.Id) ??
                throw new Exception("Failed to retrieve the created document.");

            return _mapper.Map<DocumentDto>(document);
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

        public async Task<PagedResultDto<DocumentDto>> GetAllDocumentsAsync(int page, int pageSize, int? courseId = null)
        {
            var query = _unitOfWork.Documents.FindAll(trackChanges: false);

            if (courseId.HasValue)
            {
                query = query.Where(d => d.CourseId == courseId.Value);
            }

            var (documents, totalCount) = await query
                .OrderByDescending(d => d.Id)
                .PagedResult(page, pageSize);

            var dtos = _mapper.Map<List<DocumentDto>>(documents);

            return new PagedResultDto<DocumentDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
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