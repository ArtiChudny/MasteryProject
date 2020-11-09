using System;
using System.IO;

namespace FileStorage
{
    public class FileManager
    {
        public void MoveFileToDestinationPath(string filePath, string destinationPath)
        {
            if (!File.Exists(filePath))
            {
                throw new ApplicationException($"Path {Path.GetFileName(filePath)} not exists");
            }
            string fullFilePath = Path.Combine(destinationPath, Path.GetFileName(filePath));

            File.Copy(filePath, fullFilePath);
        }
    }
}
