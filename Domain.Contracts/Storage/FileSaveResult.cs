namespace Domain.Contracts.Storage;

public record FileSaveResult(
    string FileName,
    string PublicPath,
    long FileSize
);