using Domain.Contracts.Storage;
using LMS.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace LMS.Infrastructure.Storage;

public class LocalFileStorage : IFileStorage
{
    private readonly FileStorageOptions _options;

    public LocalFileStorage(IOptions<FileStorageOptions> options)
    {
        _options = options.Value;
    }

    public async Task<FileSaveResult> SaveAsync(
        Stream stream,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required.", nameof(fileName));

        if (string.IsNullOrWhiteSpace(_options.UploadRootPath))
            throw new InvalidOperationException("FileStorageOptions.UploadRootPath is not configured.");

        Directory.CreateDirectory(_options.UploadRootPath);

        var extension = Path.GetExtension(fileName);
        var storedFileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(_options.UploadRootPath, storedFileName);

        await using var fileStream = new FileStream(fullPath, FileMode.Create);
        await stream.CopyToAsync(fileStream, cancellationToken);

        return new FileSaveResult(
            FileName: storedFileName,
            PublicPath: $"{_options.RequestBasePath.TrimEnd('/')}/{storedFileName}",
            FileSize: fileStream.Length
        );
    }
}