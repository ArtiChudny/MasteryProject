using FileStorage.DAL.Helpers;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private readonly string _storageInfoPath = ConfigurationManager.AppSettings["StorageInfoPath"];
        private readonly long _maxStorage = Convert.ToInt64(ConfigurationManager.AppSettings["MaxStorage"]);
        private readonly long _maxFileSize = Convert.ToInt64(ConfigurationManager.AppSettings["MaxFileSize"]);
        private readonly BinaryFormatter _binaryFormatter;

        public StorageRepository()
        {
            _binaryFormatter = new BinaryFormatter();
        }

        public Task<StorageFile> CreateFile(string directoryPath, string fileName, long fileSize, byte[] hash, DateTime creationDate)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageDirectory directory = GetDirectoryByPath(directoryPath, storageInfo.Directories);

            if (directory.Files.ContainsKey(fileName))
            {
                throw new ArgumentException($"The file '{fileName}' is already exists in the '{directoryPath}'");
            }

            var storageFile = new StorageFile
            {
                Id = Guid.NewGuid(),
                CreationDate = creationDate,
                Extension = Path.GetExtension(fileName),
                Size = fileSize,
                Hash = hash,
                DownloadsNumber = 0
            };

            directory.Files.Add(fileName, storageFile);
            storageInfo.UsedStorage += fileSize;
            SerializeStorageInfoFile(storageInfo);

            return Task.FromResult(storageFile);
        }

        public Task<StorageFile> GetFileInfo(string filePath)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            var storageFile = GetFileByPath(storageInfo, filePath);

            return Task.FromResult(storageFile);
        }

        public Task<StorageInfo> GetStorageInfo()
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            return Task.FromResult(storageInfo);
        }

        private StorageFile GetFileByPath(StorageInfo storageInfo, string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            string parentDirectoryPath = PathHelper.GetParentDirectoryPath(filePath);

            var directory = GetDirectoryByPath(parentDirectoryPath, storageInfo.Directories);

            return directory.Files.ContainsKey(fileName) ? directory.Files[fileName] : null;
        }

        public void IncreaseDownloadsCounter(string filePath)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            var storageFile = GetFileByPath(storageInfo, filePath);
            storageFile.DownloadsNumber++;

            SerializeStorageInfoFile(storageInfo);
        }

        public void InitializeStorage()
        {
            string storagePath = Path.GetDirectoryName(_storageInfoPath);

            if (!Directory.Exists(storagePath))
            {
                throw new ArgumentException($"Missing path '{Path.GetFullPath(storagePath)}'");
            }

            if (!File.Exists(_storageInfoPath))
            {
                StorageInfo storageInfo = new StorageInfo();
                SerializeStorageInfoFile(storageInfo);
            }
        }

        public bool IsEnoughStorageSpace(long fileSize)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            return (storageInfo.UsedStorage + fileSize) < _maxStorage;
        }

        public bool IsFileSizeLessThanMaxSize(long fileSize)
        {
            return fileSize < _maxFileSize;
        }

        public Task MoveFile(string oldFilePath, string newFilePath)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageFile movableStorageFile = GetFileByPath(storageInfo, oldFilePath);

            if (movableStorageFile == null)
            {
                throw new ArgumentException($"The file '{oldFilePath}' is not exists");
            }

            string newFileName = Path.GetFileName(newFilePath);
            string newParentDerictoryPath = PathHelper.GetParentDirectoryPath(newFilePath);

            StorageDirectory newParentDirectory = GetDirectoryByPath(newParentDerictoryPath, storageInfo.Directories);

            if (newParentDirectory.Files.ContainsKey(newFileName))
            {
                throw new ArgumentException($"The file '{newFileName}' is already exists in directory {newParentDerictoryPath}");
            }

            string oldFileName = Path.GetFileName(oldFilePath);
            string oldParentDerictoryPath = PathHelper.GetParentDirectoryPath(oldFilePath);
            StorageDirectory oldParentDirectory = GetDirectoryByPath(oldParentDerictoryPath, storageInfo.Directories);
            oldParentDirectory.Files.Remove(oldFileName);

            movableStorageFile.Extension = Path.GetExtension(newFileName);
            newParentDirectory.Files.Add(newFileName, movableStorageFile);

            SerializeStorageInfoFile(storageInfo);

            return Task.CompletedTask;
        }

        public Task RemoveFile(string filePath)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            string fileName = Path.GetFileName(filePath);
            string parentDirectoryPath = PathHelper.GetParentDirectoryPath(filePath);
            StorageDirectory parentDirectory = GetDirectoryByPath(parentDirectoryPath, storageInfo.Directories);

            storageInfo.UsedStorage -= parentDirectory.Files[fileName].Size;
            parentDirectory.Files.Remove(fileName);

            SerializeStorageInfoFile(storageInfo);

            return Task.CompletedTask;
        }

        private void SerializeStorageInfoFile(StorageInfo storageInfo)
        {
            using (FileStream fileStream = new FileStream(_storageInfoPath, FileMode.OpenOrCreate))
            {
                _binaryFormatter.Serialize(fileStream, storageInfo);
            }
        }

        private StorageInfo DeserializeStorageInfoFile()
        {
            using (FileStream fileStream = new FileStream(_storageInfoPath, FileMode.OpenOrCreate))
            {
                StorageInfo storageInfo = (StorageInfo)_binaryFormatter.Deserialize(fileStream);

                return storageInfo;
            }
        }

        public Task CreateDirectory(string path, string directoryName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageDirectory parentDirectory = GetDirectoryByPath(path, storageInfo.Directories);

            if (parentDirectory.Directories.ContainsKey(directoryName))
            {
                throw new ArgumentException($"The directory {directoryName} is already exists at the path '{path}'");
            }

            parentDirectory.Directories.Add(directoryName, new StorageDirectory()
            {
                Name = directoryName,
                ParentId = parentDirectory.Name
            });

            SerializeStorageInfoFile(storageInfo);

            return Task.CompletedTask;
        }

        private StorageDirectory GetDirectoryByPath(string path, Dictionary<string, StorageDirectory> directories)
        {
            var pathArray = path.Remove(0, 1).Split("/");

            if (pathArray.Length != 0 && directories.ContainsKey(pathArray[0]))
            {
                if (pathArray.Length == 1)
                {
                    return directories[pathArray[0]];
                }
                else
                {
                    string tmpDirName = pathArray[0];
                    path = path.Remove(0, tmpDirName.Length + 1);
                    return GetDirectoryByPath(path, directories[tmpDirName].Directories);
                }
            }

            throw new ArgumentException("The path doesn't exist");
        }

        public Task MoveDirectory(string oldPath, string newPath)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            //Getting old directory
            StorageDirectory movableDirectory = GetDirectoryByPath(oldPath, storageInfo.Directories);

            //Getting old parent directory
            string oldParentDirectoryPath = PathHelper.GetParentDirectoryPath(oldPath);
            StorageDirectory oldParentDirectory = GetDirectoryByPath(oldParentDirectoryPath, storageInfo.Directories);

            //Getting new parent directory
            string newDirectoryName = Path.GetFileName(newPath);
            string newParentDirectoryPath = PathHelper.GetParentDirectoryPath(newPath);
            StorageDirectory newParentDirectory = GetDirectoryByPath(newParentDirectoryPath, storageInfo.Directories);

            if (newParentDirectory.Directories.ContainsKey(newDirectoryName))
            {
                throw new InvalidOperationException($"The directory {newDirectoryName} is already exists at the path '{newPath}'");
            }
            else
            {
                oldParentDirectory.Directories.Remove(movableDirectory.Name);
                movableDirectory.Name = newDirectoryName;
                movableDirectory.ParentId = newParentDirectory.Name;
                newParentDirectory.Directories.Add(newDirectoryName, movableDirectory);
            }

            SerializeStorageInfoFile(storageInfo);

            return Task.CompletedTask;
        }

        public Task<StorageDirectory> GetDirectory(string path)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            var storageDirectory = GetDirectoryByPath(path, storageInfo.Directories);

            return Task.FromResult(storageDirectory);
        }

        public Task RemoveDirectory(string path)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            string directoryName = Path.GetFileName(path);
            string parentDirectoryPath = PathHelper.GetParentDirectoryPath(path);
            StorageDirectory parentDirectory = GetDirectoryByPath(parentDirectoryPath, storageInfo.Directories);

            if (!parentDirectory.Directories.ContainsKey(directoryName))
            {
                throw new ArgumentException("The path doesn't exist");
            }

            parentDirectory.Directories.Remove(directoryName);
            SerializeStorageInfoFile(storageInfo);

            return Task.CompletedTask;
        }
    }
}
