using BeautyMap.FileManager.Models;

namespace BeautyMap.FileManager.Interfaces
{
    public interface IFileRetrieveService
    {
        Task<List<FileRetrieveModel>> RetrieveFiles(string folder, bool notAddItSelf = false);
        Task<List<FileRetrieveModel>> RetrieveFilesRecursive(string folder);
        Task<List<FileRetrieveModel>> SearchFiles(string searchKey, string folderName);

        Task<List<string>> RetrieveHomePage(string searchKey);
        string GeneratePreSignedUrl(string filePath, DateTime expirationDate);
    }
}
