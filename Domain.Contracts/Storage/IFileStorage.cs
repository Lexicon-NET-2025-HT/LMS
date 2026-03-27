namespace Domain.Contracts.Storage;

public interface IFileStorage
{
    Task<FileSaveResult> SaveAsync(Stream stream, string fileName, CancellationToken cancellationToken = default);
}