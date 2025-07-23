using BeautyMap.Domain.Common.BaseEntities;
using Microsoft.VisualBasic.FileIO;

namespace BeautyMap.Domain.Entities.Files
{
    public partial class File : TrackedEntity<int>
    {
        protected File() { }
        public File(
            string originalName,
            string originalExtension,
            string name,
            string downloadUrl,
            string mimeType,
            long lengthInBytes,
            int fileTypeId,
            string path,
            File parent)
        {
            OriginalName = originalName;
            OriginalExtension = originalExtension;
            Name = name;
            MimeType = mimeType;
            LengthInBytes = lengthInBytes;
            FileTypeId = fileTypeId;
            ParentFile = parent;
            Path = path;
            Extension = originalExtension;
            AssignDownloadUrl(downloadUrl);
        }
        private void AssignDownloadUrl(string downloadUrl)
        {
            // Define the part of the path we want to replace (the root of the project)
            string basePath = @"C:\Inetpub\vhosts\sadagi.ge\httpdocs\assets\StoredImages\";

            // Check if the path starts with the base path (case insensitive)
            if (downloadUrl.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
            {
                // Extract the relative path by removing the base path portion
                string relativePath = downloadUrl.Substring(basePath.Length);

                // Replace backslashes with forward slashes for URL compatibility
                relativePath = relativePath.Replace('\\', '/');

                // Construct the final URL
                DownloadUrl = $"https://sadagi.ge/assets/StoredImages/{relativePath}";
            }
            else
            {
                // If the path doesn't match the expected base path, use the original download URL
                DownloadUrl = downloadUrl;
            }
        }
        public string OriginalName { get; set; }
        public string OriginalExtension { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string DownloadUrl { get; set; }
        public string MimeType { get; set; }
        public long LengthInBytes { get; set; }
        public string? Path { get; set; }

        #region Relationship

        #region FileType
        public int FileTypeId { get; set; }
        public virtual FileType FileType { get; set; }

        #endregion

        #region Child File
        public int? ParentId { get; set; } = null;
        public File ParentFile { get; set; }
        #endregion

        #endregion
    }
}