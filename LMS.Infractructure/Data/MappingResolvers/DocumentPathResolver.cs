using AutoMapper;
using Domain.Models.Entities;
using LMS.Infrastructure.Options;
using LMS.Shared.DTOs.Document;
using Microsoft.Extensions.Options;

namespace LMS.Infractructure.Data.MappingResolvers;

public class DocumentPathResolver : IValueResolver<Document, DocumentDto, string>
{
    private readonly FileStorageOptions _options;

    public DocumentPathResolver(IOptions<FileStorageOptions> options)
    {
        _options = options.Value;
    }

    public string Resolve(Document source, DocumentDto destination, string destMember, ResolutionContext context)
    {
        return $"{_options.RequestBasePath.TrimEnd('/')}/{source.StoredFileName}";
    }
}
