using FileStorage.DAL.Constants;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileStorage.DAL.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly string _storageFilesPath = ConfigurationManager.AppSettings["StorageFilesPath"];

        public Task DownloadFileFromStorage(string fileName, string guidFileName, string destinationPath)
        {
            if (!Directory.Exists(destinationPath))
            {
                throw new ArgumentException($"Directory '{destinationPath}' is not exists");
            }

            string fullStorageFilePath = Path.Combine(_storageFilesPath, guidFileName);
            string fullDestinationFilePath = Path.Combine(destinationPath, fileName);
            File.Copy(fullStorageFilePath, fullDestinationFilePath);

            return Task.CompletedTask;
        }

        public Task ExportFile(SerializableStorageInfo storageInfo, string destinationPath, string format)
        {
            string directoryPath = Path.GetDirectoryName(destinationPath);

            if (!Directory.Exists(directoryPath))
            {
                throw new ArgumentException($"Directory '{directoryPath}' is not exists");
            }

            if (string.IsNullOrWhiteSpace(format))
            {
                format = FileFormats.Json;
            }

            switch (format)
            {
                case FileFormats.Json:
                    {
                        ExportFileToJSON(storageInfo, destinationPath);
                        break;
                    }
                case FileFormats.Xml:
                    {
                        ExportFileToXML(storageInfo, destinationPath);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException($"Unknown format '{format}'");
                    }
            }

            return Task.CompletedTask;
        }

        public Task<FileInfoModel> GetFileInfo(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File '{filePath}' is not exists");
            }

            FileInfo fileInfo = new FileInfo(filePath);

            var fileInfoModel = new FileInfoModel
            {
                CreationDate = fileInfo.CreationTime,
                Size = fileInfo.Length,
                Hash = GetFileHash(filePath)
            };

            return Task.FromResult(fileInfoModel);
        }

        public byte[] GetFileHash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        public void InitializeFileStorage()
        {
            if (!Directory.Exists(_storageFilesPath))
            {
                throw new ArgumentException($"Missing path '{Path.GetFullPath(_storageFilesPath)}'");
            }
        }

        public bool IsHashMatch(string fileName, byte[] storageFileHash)
        {
            string filePath = Path.Combine(_storageFilesPath, fileName);

            if (!File.Exists(filePath))
            {
                throw new ArgumentException("Could not find physical file. Maybe it was removed without using the app");
            }

            byte[] fileHash = GetFileHash(filePath);

            if (fileHash.SequenceEqual(storageFileHash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task MoveFile(string oldFileName, string newFileName)
        {
            string filePath = Path.Combine(_storageFilesPath, oldFileName);

            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File '{oldFileName}' is not exists");
            }

            string newFilePath = Path.Combine(_storageFilesPath, newFileName);

            if (File.Exists(newFilePath))
            {
                throw new ArgumentException($"File '{newFileName}' is already exist in storage");
            }

            File.Move(filePath, newFilePath);

            return Task.CompletedTask;
        }

        public Task RemoveFile(string fileName)
        {
            string filePath = Path.Combine(_storageFilesPath, fileName);

            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"Could not find physical file. Maybe it was already removed without using the app");
            }

            File.Delete(filePath);

            return Task.CompletedTask;
        }

        public Task UploadFileIntoStorage(string filePath, string guid)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File with such directory '{Path.GetFileName(filePath)}' is not exists");
            }

            string fullDestinationPath = Path.Combine(_storageFilesPath, guid);
            File.Copy(filePath, fullDestinationPath);

            return Task.CompletedTask;
        }

        private void ExportFileToXML(SerializableStorageInfo storageInfo, string destinationPath)
        {
            using (FileStream fileStream = new FileStream(destinationPath, FileMode.OpenOrCreate))
            {
                XmlSerializer formatter = new XmlSerializer(storageInfo.GetType());
                formatter.Serialize(fileStream, storageInfo);
            }
        }

        private void ExportFileToJSON(SerializableStorageInfo storageInfo, string destinationPath)
        {
            string jsonString = JsonSerializer.Serialize(storageInfo);
            File.WriteAllText(destinationPath, jsonString);
        }
    }
}
