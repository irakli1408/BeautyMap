using BeautyMap.FileManager.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BeautyMap.FileManager.Services
{
    public class FileEditService : IFileEditService
    {
        private readonly IConfiguration _configuration;
        private readonly string _baseFolder;

        public FileEditService(IConfiguration configuration)
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

        public async Task EditFolder(string folderPath, string folderNewName)
        {
            var fullPath = Path.Combine(_baseFolder, folderPath);
            var newFullPath = Path.Combine(_baseFolder, folderNewName);

            if (!Directory.Exists(fullPath))
            {
                throw new Exception("Folder Not Found");
            }

            if (Directory.Exists(newFullPath))
            {
                throw new Exception("Folder already exists");
            }

            try
            {
                Directory.Move(fullPath, newFullPath);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task MoveToRecycleBin(string fileName)
        {
            var fullPath = Path.Combine(_baseFolder, fileName);
            var recycleBinPath = Path.Combine(_baseFolder, "ნაგვის ურნა", Path.GetFileName(fileName));

            if (!File.Exists(fullPath))
            {
                throw new Exception("File does not exist");
            }

            try
            {
                // Ensure the recycle bin directory exists
                var recycleBinDirectory = Path.Combine(_baseFolder, "ნაგვის ურნა");
                if (!Directory.Exists(recycleBinDirectory))
                {
                    Directory.CreateDirectory(recycleBinDirectory);
                }

                // Move the file to the recycle bin
                File.Move(fullPath, recycleBinPath);

                // Simulate async operation
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task RestoreFromRecycleBin(string originalPath)
        {
            var fileName = Path.GetFileName(originalPath);
            var recycleBinPath = Path.Combine(_baseFolder, "ნაგვის ურნა", fileName);
            var fullPath = Path.Combine(_baseFolder, originalPath);

            if (!File.Exists(recycleBinPath))
            {
                throw new Exception("File Does not exist");
            }

            try
            {
                // Ensure the original directory exists
                var originalDirectory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(originalDirectory))
                {
                    Directory.CreateDirectory(originalDirectory);
                }

                // Move the file from the recycle bin to the original path
                File.Move(recycleBinPath, fullPath);

                // Simulate async operation
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
