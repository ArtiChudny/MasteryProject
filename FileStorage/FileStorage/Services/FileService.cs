using FileStorage.Models;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace FileStorage
{
    public class FileService
    {
        string storagePath = ConfigurationManager.AppSettings["StoragePath"];
        string storageInfoPath = ConfigurationManager.AppSettings["StorageInfoPath"];

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

        public void UploadFileIntoStorage(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ApplicationException($"Path {Path.GetFileName(filePath)} is not exists");
            }

            string fullDestinationPath = Path.Combine(storagePath, Path.GetFileName(filePath));
            File.Copy(filePath, fullDestinationPath);
        }

        public void DownloadFileFromStorage(string fileName, string destinationPath)
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
            string fullDestinationFilePath = Path.Combine(destinationPath, fileName);
            File.Copy(fullStorageFilePath, fullDestinationFilePath);
        }

        public void MoveFile(string oldFileName, string newFileName)
        {
            string filePath = Path.Combine(storagePath, oldFileName);

            if (!File.Exists(filePath))
            {
                throw new ApplicationException($"File {oldFileName} is not exists");
            }

            string newFilePath = Path.Combine(storagePath, newFileName);

            if (File.Exists(newFilePath))
            {
                throw new ApplicationException($"File {newFileName} is already exist in storage");
            }

            File.Move(filePath, newFilePath);
        }

        public void RemoveFile(string fileName)
        {
            string filePath = Path.Combine(storagePath, fileName);

            if (!File.Exists(filePath))
            {
                throw new ApplicationException($"File {fileName} is not exists");
            }

            File.Delete(filePath);
        }

        public StorageFile GetStorageFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            StorageFile storageFile = new StorageFile
            {
                CreationDate = fileInfo.CreationTime,
                Extension = fileInfo.Extension,
                FileName = fileInfo.Name,
                Size = fileInfo.Length,
                DownloadsNumber = 0,
                Md5Hash = GetMD5Hash(filePath)
            };

            return storageFile;
        }

        private byte[] GetMD5Hash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        public bool IsMd5HashMatch(string fileName, byte[] storageFileMd5Hash)
        {
            string filePath = Path.Combine(storagePath, fileName);
            byte[] fileMD5Hash = GetMD5Hash(filePath);
            if (fileMD5Hash.SequenceEqual(storageFileMd5Hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
