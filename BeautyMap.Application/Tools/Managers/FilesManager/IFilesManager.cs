using BeautyMap.Application.Base.BeautyMap.Application.Common.Base;
using File = BeautyMap.Domain.Entities.Files.File;

namespace BeautyMap.Application.Tools.Managers.FilesManager
{
    public interface IFilesManager
    {
        Task<File> RetrieveFile(int fileId, bool forAdmin = true);
        Task<List<File>> RetrieveFiles(List<int> fileIds, bool forAdmin = true);
        FileResponse RetrieveFileResponse(File file, bool forAdmin = true);
        FileResponse RetrieveFileResponse(int fileId, bool forAdmin = true);
        List<FileResponse> RetrieveFilesResponse(IEnumerable<File> files, bool forAdmin = true);

        FileResponse RetrieveFileResponse(File file);
        IEnumerable<FileResponse> RetrieveFilesResponse(IEnumerable<File> files);

        string CreateNewPath(string oldPath, string newPath);
        string CreateNewDownloadUrl(string oldPath, string newPath);
    }
}
