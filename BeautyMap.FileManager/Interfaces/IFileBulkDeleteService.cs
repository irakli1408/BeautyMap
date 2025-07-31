namespace BeautyMap.FileManager.Interfaces
{
    public interface IFileBulkDeleteService : IFileDeleteService
    {
        Task BulkFileDelete(List<string> fileName);
        Task BulkFolderDelete(List<string> folderName);
    }
}
