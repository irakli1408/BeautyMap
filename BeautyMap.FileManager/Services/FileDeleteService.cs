using BeautyMap.FileManager.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BeautyMap.FileManager.Services
{
    internal class FileDeleteService : IFileBulkDeleteService
    {
        private readonly IConfiguration _configuration;
        private readonly string _baseFolder;

        public FileDeleteService(IConfiguration configuration)
        {
            _configuration = configuration;

            // Get the current directory where the app is running (e.g., ../1/2/3/4)
            var currentDirectory = Directory.GetCurrentDirectory();

            // Move one level up from the current directory (e.g., ../1/2/3)
            var parentDirectory = Directory.GetParent(currentDirectory)?.FullName;

            if (parentDirectory == null)
            {
                throw new DirectoryNotFoundException("Unable to determine the parent directory.");
            }

            // Define the path for the Images folder in the parent directory (../1/2/3/Images)
            _baseFolder = Path.Combine(parentDirectory, "httpdocs", "assets", "StoredImages");

            // Ensure the base folder (Images) exists, create it if it doesn't
            if (!Directory.Exists(_baseFolder))
            {
                Directory.CreateDirectory(_baseFolder);
            }
        }

        public async Task BulkFileDelete(List<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                var filePath = Path.Combine(_baseFolder, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            // Simulating async behavior
            await Task.CompletedTask;
        }

        public async Task BulkFolderDelete(List<string> folderNames)
        {
            foreach (var folderName in folderNames)
            {
                var folderPath = Path.Combine(_baseFolder, folderName);
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true); // true to delete recursively
                }
            }

            // Simulating async behavior
            await Task.CompletedTask;
        }

        public async Task Delete(string name, bool isFolder)
        {
            var path = Path.Combine(_baseFolder, name);

            if (isFolder)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            else
            {
                var filePath = path + _configuration["FileDefaultExtension"];
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            await Task.CompletedTask;
        }
    }
}
