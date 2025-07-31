using BeautyMap.FileManager.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;

namespace BeautyMap.FileManager.Services
{
    internal class FileUploadService : IFileUploadService
    {
        private readonly IConfiguration _configuration;
        private readonly string _baseFolder;

        public FileUploadService(IConfiguration configuration)
        {
            _configuration = configuration;

            // Get the current directory where the app is running (e.g., ../1/2/3/4)
            var currentDirectory = Directory.GetCurrentDirectory();

            // Move one level up from the current directory (e.g., ../1/2/3)
            var parentDirectory = (Directory.GetParent(currentDirectory)?.FullName) ?? throw new DirectoryNotFoundException("Unable to determine the parent directory.");

            // Define the path for the Images folder in the parent directory (../1/2/3/Images)
            _baseFolder = Path.Combine(parentDirectory, "httpdocs", "assets", "StoredImages");

            // Ensure the base folder (Images) exists, create it if it doesn't
            if (!Directory.Exists(_baseFolder))
            {
                Directory.CreateDirectory(_baseFolder);
            }
        }

        public async Task<(string parentPath, string childPath)> UploadAsync(IFormFile request, string folderName, CancellationToken cancellationToken)
        {
            if (request.Length <= 0)
            {
                throw new Exception("File has no length!");
            }

            var parentGuid = Guid.NewGuid().ToString();
            var childGuid = Guid.NewGuid().ToString();

            // Save original image
            var parentPath = await UploadImageAsync(request, parentGuid, cancellationToken, resize: false, folderName);

            // Save resized image
            var childPath = await UploadImageAsync(request, childGuid, cancellationToken, resize: true, folderName);

            return (parentPath, childPath);
        }

        private async Task<string> UploadImageAsync(IFormFile file, string uniqueId, CancellationToken cancellationToken, bool resize = false, string folderName = null)
        {
            await using var outputStream = new MemoryStream();

            using (var image = await Image.LoadAsync(file.OpenReadStream(), cancellationToken))
            {
                if (resize)
                {
                    int targetWidth = Convert.ToInt32(_configuration["Image:Width"]);
                    int targetHeight = Convert.ToInt32(_configuration["Image:Height"]);

                    var scalingFactor = Math.Min((double)targetWidth / image.Width, (double)targetHeight / image.Height);
                    var newDimensions = new Size((int)(image.Width * scalingFactor), (int)(image.Height * scalingFactor));

                    image.Mutate(x => x.Resize(newDimensions));
                }

                string extension = Path.GetExtension(file.FileName);
                var encoder = image.DetectEncoder(extension);
                await image.SaveAsync(outputStream, encoder, cancellationToken);
            }

            outputStream.Position = 0;

            // Define the folder path based on the folder name
            var keyPrefix = string.IsNullOrEmpty(folderName) ? "" : $"{folderName.TrimEnd('\\')}\\";
            var folderPath = Path.Combine(_baseFolder, keyPrefix);

            // Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Define the file name and path (e.g., ../1/2/3/Images/folderName/uniqueId.extension)
            var fileName = $"{uniqueId}{Path.GetExtension(file.FileName.ToLower())}";
            var filePath = Path.Combine(folderPath, fileName);

            try
            {
                // Write the file to the local folder
                await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                outputStream.Position = 0;
                await outputStream.CopyToAsync(fileStream, cancellationToken);
                return filePath;
            }
            catch (IOException ex)
            {
                throw new Exception($"Error saving file: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
