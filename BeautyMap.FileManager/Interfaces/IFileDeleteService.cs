namespace BeautyMap.FileManager.Interfaces
{
    public interface IFileDeleteService
    {
        Task Delete(string name, bool IsFolder);
    }
}
