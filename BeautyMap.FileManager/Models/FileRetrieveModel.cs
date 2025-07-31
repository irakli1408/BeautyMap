namespace BeautyMap.FileManager.Models
{
    public class FileRetrieveModel
    {
        public bool IsFolder { get; set; } = false;
        public string Name { get; set; }
        public FileRetrieveModel(string name, bool isFolder = false)
        {
            Name = name;
            IsFolder = isFolder;
        }
    }
}
