using BeautyMap.FileManager.Interfaces;
using BeautyMap.FileManager.Models;
using Microsoft.Extensions.Configuration;

namespace BeautyMap.FileManager.Services
{
    public class FileRetrieveService : IFileRetrieveService
    {
        private readonly IConfiguration _configuration;
        private readonly string _baseFolder;

        public FileRetrieveService(IConfiguration configuration)
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

        public async Task<List<FileRetrieveModel>> RetrieveFiles(string folder, bool removeFirst = false)
        {
            var fullPath = string.IsNullOrEmpty(folder) ? _baseFolder : Path.Combine(_baseFolder, folder);

            if (!Directory.Exists(fullPath))
            {
                return new List<FileRetrieveModel>();
            }

            var fileList = new List<FileRetrieveModel>();

            foreach (var dir in Directory.GetDirectories(fullPath))
            {
                fileList.Add(new FileRetrieveModel(dir, true));
            }

            foreach (var file in Directory.GetFiles(fullPath))
            {
                fileList.Add(new FileRetrieveModel(file, false));
            }

            if (removeFirst && fileList.Count > 0)
            {
                fileList.RemoveAt(0);
            }

            // Simulating async behavior
            await Task.CompletedTask;
            return fileList;
        }

        public async Task<List<FileRetrieveModel>> RetrieveFilesRecursive(string folder)
        {
            var fullPath = Path.Combine(_baseFolder, folder);

            if (!Directory.Exists(fullPath))
            {
                return new List<FileRetrieveModel>();
            }

            var files = new List<FileRetrieveModel>();
            var recursiveFiles = await RetrieveFilesInFolder(fullPath);
            files.AddRange(recursiveFiles);

            return files;
        }

        public async Task<List<FileRetrieveModel>> SearchFiles(string searchKey, string folderName)
        {
            var fullPath = string.IsNullOrEmpty(folderName) ? _baseFolder : Path.Combine(_baseFolder, folderName);

            if (!Directory.Exists(fullPath))
            {
                return new List<FileRetrieveModel>();
            }

            var fileList = new List<FileRetrieveModel>();
            var allFiles = Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories)
            .Where(file => file.Contains(searchKey, StringComparison.OrdinalIgnoreCase))
            .ToList();

            foreach (var file in allFiles)
            {
                // Check if searchKey is in the file name or elsewhere in the path
                var fileName = Path.GetFileName(file);
                var isInFileName = fileName.Contains(searchKey, StringComparison.OrdinalIgnoreCase);

                if (isInFileName)
                {
                    // If searchKey is in the file name
                    fileList.Add(new FileRetrieveModel(file, false));
                }
                else
                {
                    // If searchKey is elsewhere in the path
                    fileList.Add(new FileRetrieveModel(file, true));
                }
            }

            // Simulating async behavior
            await Task.CompletedTask;
            return fileList;
        }

        public async Task<List<string>> RetrieveHomePage(string searchKey)
        {
            if (!Directory.Exists(_baseFolder))
            {
                return new List<string>();
            }

            var folders = Directory.GetDirectories(_baseFolder)
                .Select(dir => dir.TrimEnd(Path.DirectorySeparatorChar))
                .Where(dir => !dir.Contains("ნაგვის ურნა") && (string.IsNullOrEmpty(searchKey) || dir.Contains(searchKey, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(dir => dir)
                .ToList();

            // Simulating async behavior
            await Task.CompletedTask;
            return folders;
        }

        public string GeneratePreSignedUrl(string filePath, DateTime expirationDate)
        {
            //ToDo: dasamatebelia rom sul ver xedavdes
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File does not exist.", filePath);
            }

            // For local files, we simply return the file path since there's no URL generation involved.
            return filePath;
        }

        #region Private

        private static async Task<List<FileRetrieveModel>> RetrieveFilesInFolder(string folderPath)
        {
            var fileList = new List<FileRetrieveModel>();

            foreach (var dir in Directory.GetDirectories(folderPath))
            {
                fileList.Add(new FileRetrieveModel(dir, true));
                var nestedFiles = await RetrieveFilesInFolder(dir);
                fileList.AddRange(nestedFiles);
            }

            foreach (var file in Directory.GetFiles(folderPath))
            {
                fileList.Add(new FileRetrieveModel(file, false));
            }

            // Simulating async behavior
            await Task.CompletedTask;
            return fileList;
        }

        #endregion
    }
}
