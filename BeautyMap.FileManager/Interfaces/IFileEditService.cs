namespace BeautyMap.FileManager.Interfaces
{
    public interface IFileEditService
    {
        Task EditFolder(string folderName, string folderNewName);
    }
}
