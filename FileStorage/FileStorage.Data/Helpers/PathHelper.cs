using System.IO;

namespace FileStorage.DAL.Helpers
{
    public static class PathHelper
    {
        public static string GetParentDirectoryPath(string fullPath)
        {
            var fileName = Path.GetFileName(fullPath);
            return fullPath.Substring(0, fullPath.LastIndexOf(fileName) - 1);
        }
    }
}
