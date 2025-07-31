using BeautyMap.FileManager.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BeautyMap.FileManager.Services
{
    public class FileCreateService : IFileCreateService
    {
        private readonly IConfiguration _configuration;
        private readonly string _baseFolder;

        public FileCreateService(IConfiguration configuration)
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

        public async Task CreateFolder(string folderName)
        {
            try
            {
                var fullPath = Path.Combine(_baseFolder, folderName);

                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
