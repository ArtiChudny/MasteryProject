using FileStorage.Models;
using System;
using System.Configuration;
using System.IO;

namespace FileStorage
{
    public class FileService
    {
        string storagePath = ConfigurationManager.AppSettings["StoragePath"];
        string storageInfoPath = ConfigurationManager.AppSettings["StorageInfoPath"];

        public StorageFile UploadFileIntoStorage(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ApplicationException($"Path {Path.GetFileName(filePath)} is not exists");
            }
            string fullDestinationPath = Path.Combine(storagePath, Path.GetFileName(filePath));
            File.Copy(filePath, fullDestinationPath);

            return GetStorageFile(filePath);
        }

        public void DownloadFileFromStorage(string fileName,string destinationPath)
        {
            string filePath = Path.Combine(storagePath, fileName);

            if (!File.Exists(filePath))
            {
                throw new ApplicationException($"File {fileName} is not exists");
            }

            if (!Directory.Exists(destinationPath))
            {
                throw new ApplicationException($"Directory {destinationPath} is not exists");
            }
            string fullStorageFilePath = Path.Combine(storagePath, fileName);
            string fullDestinationPath = Path.Combine(destinationPath, fileName);
            File.Copy(fullStorageFilePath, fullDestinationPath);
        }

        private StorageFile GetStorageFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            StorageFile storageFile = new StorageFile
            {
                CreationDate = fileInfo.CreationTime,
                Extension = fileInfo.Extension,
                FileName = fileInfo.Name,
                Size = fileInfo.Length,
                DownloadsNumber = 0
            };

            return storageFile;
        }

        public void CreateIfMissInitialDirectories()
        {
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }
            if (!Directory.Exists(Path.GetDirectoryName(storageInfoPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(storageInfoPath));
            }
        }
    }
}
