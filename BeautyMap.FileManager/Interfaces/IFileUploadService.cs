using Microsoft.AspNetCore.Http;

namespace BeautyMap.FileManager.Interfaces
{
    public interface IFileUploadService
    {
        Task<(string parentPath, string childPath)> UploadAsync(IFormFile file, string folderName, CancellationToken cancellationToken);
    }
}
