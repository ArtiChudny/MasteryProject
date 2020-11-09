using FileStorage.Models;
using System;
using System.IO;

namespace FileStorage
{
    public class FileManager
    {
        public StorageFile MoveFileToDestinationPath(string filePath, string destinationPath)
        {
            if (!File.Exists(filePath))
            {
                throw new ApplicationException($"Path {Path.GetFileName(filePath)} not exists");
            }
            string fullFilePath = Path.Combine(destinationPath, Path.GetFileName(filePath));

            File.Copy(filePath, fullFilePath);

            return GetFileInfo(filePath);
        }

        public StorageFile GetFileInfo(string filePath)
        {
            var f = new FileInfo(filePath);
            StorageFile storageFile = new StorageFile
            {
                CreationDate = f.CreationTime,
                Extension = f.Extension,
                FileName = f.Name,
                Size = f.Length,
                DownloadsNumber = 0
            };

            return storageFile;
        }
    }
}
