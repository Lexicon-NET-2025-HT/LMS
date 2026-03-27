namespace LMS.Infrastructure.Options;

public class FileStorageOptions
{
    public string UploadRootPath { get; set; } = string.Empty;
    public string RequestBasePath { get; set; } = "/uploads";
}