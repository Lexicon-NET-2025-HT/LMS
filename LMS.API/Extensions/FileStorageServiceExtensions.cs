using Domain.Contracts.Storage;
using LMS.Infrastructure.Options;
using LMS.Infrastructure.Storage;

namespace LMS.API.Extensions;

public static class FileStorageServiceExtensions
{
    public static IServiceCollection AddFileStorage(
        this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        var uploadRootPath = Path.Combine(environment.ContentRootPath, "wwwroot", "uploads");

        services.Configure<FileStorageOptions>(options =>
        {
            options.UploadRootPath = uploadRootPath;
            options.RequestBasePath = "/uploads";
        });

        services.AddScoped<IFileStorage, LocalFileStorage>();

        return services;
    }
}