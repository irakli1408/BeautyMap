using BeautyMap.Application.Base.BlogLike.Application.Common.Base;
using BeautyMap.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text;
using File = BeautyMap.Domain.Entities.Files.File;

namespace BeautyMap.Application.Tools.Managers.FilesManager
{
    public class FilesManager : IFilesManager
    {
        private readonly IBlogLikeDbContext db;
        public FilesManager(IBlogLikeDbContext db)
        {
            this.db = db;
        }
        public async Task<File> RetrieveFile(int fileId, bool forAdmin = true)
        {
            var file = await RetrieveFile(fileId);

            return file;
        }
        public async Task<List<File>> RetrieveFiles(List<int> fileIds, bool forAdmin = true)
        {
            var files = await RetrieveFiles(fileIds);

            return files;
        }

        public FileResponse RetrieveFileResponse(File file, bool forAdmin = true)
        {
            if (file == null)
                return default;

            return new FileResponse(file.Id, file.Path, file.OriginalName, file.DownloadUrl, file.ParentFile.DownloadUrl);
        }

        public FileResponse RetrieveFileResponse(int fileId, bool forAdmin = true)
        {
            var file = RetrieveFile(fileId).GetAwaiter().GetResult();
            if (file == null)
                return default;

            return new FileResponse(file.Id, file.Path, file.OriginalName, file.DownloadUrl, file.ParentFile.DownloadUrl);
        }

        public List<FileResponse> RetrieveFilesResponse(IEnumerable<File> files, bool forAdmin = true)
        {
            List<FileResponse> result = new();
            foreach (var file in files)
            {
                result.Add(RetrieveFileResponse(file, forAdmin));
            }
            return result;
        }

        public FileResponse RetrieveFileResponse(File file)
        {
            if (file == null)
            {
                return new FileResponse();
            }

            if (file.ParentFile == null)
            {
                var originalFile = RetrieveFile(file.Id).GetAwaiter().GetResult();
                var originalFileDownloadUrl = originalFile?.ParentFile?.DownloadUrl;
                return new FileResponse(file.Id, file.Path, file.OriginalName, file.DownloadUrl, originalFileDownloadUrl);
            }
            return new FileResponse(file.Id, file.Path, file.OriginalName, file.DownloadUrl, file.ParentFile.DownloadUrl);
        }

        public IEnumerable<FileResponse> RetrieveFilesResponse(IEnumerable<File> files)
        {
            List<FileResponse> result = new();
            foreach (var file in files)
            {
                result.Add(RetrieveFileResponse(file));
            }
            return result;
        }

        public string CreateNewPath(string oldPath, string newPath)
        {
            oldPath = oldPath.Trim('\\');
            newPath = newPath.Trim('\\');

            string[] oldSegments = oldPath.Split('\\');
            string[] newSegments = newPath.Split('\\');

            if (oldSegments.Length < newSegments.Length)
                throw new Exception("New path is bigger than old path");

            StringBuilder resultPath = new();
            for (int i = 0; i <= oldSegments.Length - 1; i++)
            {
                if (newSegments.Length - 1 >= i)
                {
                    resultPath.Append(newSegments[i]);
                }
                else
                {
                    resultPath.Append(oldSegments[i]);
                }
                if (i != oldSegments.Length - 1)
                    resultPath.Append('\\');
            }

            return resultPath.ToString();
        }

        public string CreateNewDownloadUrl(string oldDownloadUrl, string newFolderPath)
        {
            const string baseDirectory = "StoredImages";

            int baseDirIndex = oldDownloadUrl.IndexOf(baseDirectory, StringComparison.OrdinalIgnoreCase);
            if (baseDirIndex == -1)
            {
                throw new ArgumentException($"The base directory '{baseDirectory}' was not found in the provided URL.", nameof(oldDownloadUrl));
            }

            string beforeBaseDir = oldDownloadUrl.Substring(0, baseDirIndex + baseDirectory.Length).TrimEnd('/');

            string afterBaseDir = oldDownloadUrl.Substring(baseDirIndex + baseDirectory.Length).TrimStart('/');

            int lastSlashIndex = afterBaseDir.LastIndexOf('/');
            string fileNameAndExtension = afterBaseDir.Substring(lastSlashIndex + 1);

            string oldFolderPath = afterBaseDir.Substring(0, lastSlashIndex);

            string newFolderUrlPath = newFolderPath.Replace("\\", "/").Trim('/');

            string newDownloadUrl = $"{beforeBaseDir}/{newFolderUrlPath}/{fileNameAndExtension}";

            return newDownloadUrl;
        }

        #region Private
        private async Task<File> RetrieveFile(int fileId)
        {
            var file = await db.Files
                .AsNoTracking()
                .Include(x => x.ParentFile)
                .FirstOrDefaultAsync(x => x.Id == fileId && x.DeleteDate == null) ?? default;
            return file;
        }
        private async Task<List<File>> RetrieveFiles(List<int> fileIds)
        {
            var distinctFileIds = fileIds.Distinct().ToList();

            var files = await db.Files
                .AsNoTracking()
                .Include(x => x.ParentFile)
                .Where(x => distinctFileIds.Contains(x.Id) && x.DeleteDate == null)
                .ToListAsync();

            if (files.Count != distinctFileIds.Count)
            {
                throw new Exception("Invalid File id or file is deleted");
            }
            var fileLookup = files.ToDictionary(file => file.Id);

            var resultFiles = fileIds.Select(id => fileLookup[id]).ToList();

            return resultFiles;
        }
        #endregion
    }
}
