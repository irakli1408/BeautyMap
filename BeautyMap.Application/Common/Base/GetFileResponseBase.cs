namespace BeautyMap.Application.Base
{
    namespace BlogLike.Application.Common.Base
    {
        public class GetFileBase
        {
            private static readonly List<string> Default = new() { "" };

            public string Name { get; set; }
            public string Extension { get; set; }
            public List<string> Path { get; set; } = new();

            public GetFileBase() : this(string.Empty) { }

            public GetFileBase(string path)
            {
                SetPathAndName(path);
            }

            public GetFileBase(string path, string name)
            {
                SetPathAndName(path);
                RemoveImageExtensions(name);
            }
            private void RemoveImageExtensions(string filename)
            {
                string[] extensions = { ".png", ".jpg", ".jpeg", ".webp" };

                foreach (string extension in extensions)
                {
                    if (filename.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                    {
                        Name = filename.Substring(0, filename.Length - extension.Length);
                        Extension = extension;
                        return;
                    }
                }
                Name = filename;
            }

            private void SetPathAndName(string path)
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    Path = Default;
                    Name = Default.First();
                    return;
                }

                string relevantPath = GetRelevantPath(path);

                var pathSegments = relevantPath.Split('\\');
                Path = pathSegments.Where(x => !String.IsNullOrEmpty(x)).ToList();
                Name = pathSegments.LastOrDefault();
            }

            static string GetRelevantPath(string path)
            {
                var baseDirectory = "StoredImages";
                int baseDirIndex = path.IndexOf(baseDirectory);

                if (baseDirIndex == -1)
                {
                    return path;
                }

                return path[(baseDirIndex + baseDirectory.Length)..].TrimStart('\\');
            }
        }

        public class GetFileResponseBase : GetFileBase
        {
            public int Id { get; set; }
            public string DownloadUrl { get; set; }
            public string DownloadOriginUrl { get; set; }

            public GetFileResponseBase(string path)
                : base(path)
            {
                Id = default;
                DownloadUrl = default;
                DownloadOriginUrl = default;
            }

            public GetFileResponseBase(int id, string path, string name, string url, string originalUrl)
                : base(path, name)
            {
                Id = id;
                DownloadUrl = url;
                DownloadOriginUrl = originalUrl;
            }
        }
        public sealed class FileResponse : GetFileResponseBase
        {
            public FileResponse()
                : base(String.Empty)
            { }
            public FileResponse(string path, bool isFolder)
                : base(path)
            {
                IsFolder = isFolder;
            }

            public FileResponse(int id, string path, string originalName, string url, int parentId, string parentName, string originalUrl)
                : base(id, path, originalName, url, originalUrl)
            {
                ParentId = parentId;
                ParentName = parentName;
            }

            public FileResponse(int id, string path, string originalName, string url, string originalUrl)
                : base(id, path, originalName, url, originalUrl)
            {
                ParentId = default;
                ParentName = default;
            }

            public int? ParentId { get; set; }
            public string? ParentName { get; }
            public bool IsFolder { get; set; } = false;
        }
    }

}
