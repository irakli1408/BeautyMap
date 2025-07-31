using BeautyMap.FileManager.Common;

namespace BeautyMap.FileManager.Tools.Extensions
{
    public static class FileSupport
    {
        public static string AdjustExtension(string key)
        {
            return SupportedImageExtensions.SupportedImageExtensionList.Any(ext => key.EndsWith(ext)) ? "" : "/";
        }
    }
}
