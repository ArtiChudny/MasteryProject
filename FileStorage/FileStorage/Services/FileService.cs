using FileStorage.Models;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace FileStorage
{
    public class FileService
    {
        private string storageFilesPath = ConfigurationManager.AppSettings["StorageFilesPath"];

        public void UploadFileIntoStorage(string filePath, string guid)
        {
            if (!File.Exists(filePath))
            {
                throw new ApplicationException($"File with such directory '{Path.GetFileName(filePath)}' is not exists");
            }

            string fullDestinationPath = Path.Combine(storageFilesPath, guid);
            File.Copy(filePath, fullDestinationPath);
        }

        public void DownloadFileFromStorage(string fileName, string storageFileName, string destinationPath)
        {
            if (!Directory.Exists(destinationPath))
            {
                throw new ApplicationException($"Directory {destinationPath} is not exists");
            }

            string fullStorageFilePath = Path.Combine(storageFilesPath, storageFileName);
            string fullDestinationFilePath = Path.Combine(destinationPath, fileName);
            File.Copy(fullStorageFilePath, fullDestinationFilePath);
        }

        public void MoveFile(string oldFileName, string newFileName)
        {
            string filePath = Path.Combine(storageFilesPath, oldFileName);

            if (!File.Exists(filePath))
            {
                throw new ApplicationException($"File {oldFileName} is not exists");
            }

            string newFilePath = Path.Combine(storageFilesPath, newFileName);

            if (File.Exists(newFilePath))
            {
                throw new ApplicationException($"File {newFileName} is already exist in storage");
            }

            File.Move(filePath, newFilePath);
        }

        public void RemoveFile(string fileName)
        {
            string filePath = Path.Combine(storageFilesPath, fileName);

            if (!File.Exists(filePath))
            {
                throw new ApplicationException($"File is not exists");
            }

            File.Delete(filePath);
        }

        public FileInfoModel GetStorageFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            return new FileInfoModel
            {
                CreationDate = fileInfo.CreationTime,
                Size = fileInfo.Length,
                Hash = GetHash(filePath)
            };
        }

        private byte[] GetHash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        public bool IsHashMatch(string fileName, byte[] storageFileHash)
        {
            string filePath = Path.Combine(storageFilesPath, fileName);
            byte[] fileHash = GetHash(filePath);
            if (fileHash.SequenceEqual(storageFileHash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InitializeFileStorage()
        {
            if (!Directory.Exists(storageFilesPath))
            {
                throw new ApplicationException($"Missing path '{Path.GetFullPath(storageFilesPath)}'");
            }
        }

        public void ExportFile(object serializableObject, string destinationPath, string format)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                format = "json";
            }

            switch (format)
            {
                case "json":
                    {
                        ExportFileToJSON(serializableObject, destinationPath);
                        break;
                    }
                case "xml":
                    {
                        ExportFileToXML(serializableObject, destinationPath);
                        break;
                    }
                default:
                    {
                        throw new ApplicationException($"Unknown format {format}");
                    }
            }
        }

        private void ExportFileToXML(object serializableObject, string destinationPath)
        {
            using (FileStream fileStream = new FileStream(destinationPath, FileMode.OpenOrCreate))
            {
                XmlSerializer formatter = new XmlSerializer(serializableObject.GetType());
                formatter.Serialize(fileStream, serializableObject);
            }
        }

        private async void ExportFileToJSON(object serializableObject, string destinationPath)
        {   
            using (FileStream fileStream = new FileStream(destinationPath, FileMode.OpenOrCreate))
            {
               await JsonSerializer.SerializeAsync(fileStream, serializableObject, serializableObject.GetType());
            }
        }
    }
}
