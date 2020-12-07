using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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

        public Task<StorageFile> CreateFile(string fileName, long fileSize, byte[] hash, DateTime creationDate)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            storageInfo.UsedStorage += fileSize;
            StorageFile storageFile = new StorageFile
            {
                Id = Guid.NewGuid(),
                CreationDate = creationDate,
                Extension = Path.GetExtension(fileName),
                Size = fileSize,
                Hash = hash,
                DownloadsNumber = 0
            };

            storageInfo.Files.Add(fileName, storageFile);
            SerializeStorageInfoFile(storageInfo);

            return Task.FromResult(storageFile);
        }

        public byte[] GetFileHash(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            return storageInfo.Files[fileName].Hash;
        }

        public Task<StorageFile> GetFileInfo(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            if (storageInfo.Files.ContainsKey(fileName))
            {
                return Task.FromResult(storageInfo.Files[fileName]);
            }
            else
            {
                return null;
            }
        }

        public Task<StorageInfo> GetStorageInfo()
        {
            var storageInfo = DeserializeStorageInfoFile();

            return Task.FromResult(storageInfo);
        }

        public void IncreaseDownloadsCounter(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            storageInfo.Files[fileName].DownloadsNumber++;
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

        public Task MoveFile(string oldFileName, string newFileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageFile storageFile = storageInfo.Files[oldFileName];
            storageInfo.Files.Remove(oldFileName);
            storageFile.Extension = Path.GetExtension(newFileName);
            storageInfo.Files.Add(newFileName, storageFile);
            SerializeStorageInfoFile(storageInfo);

            return Task.CompletedTask;
        }

        public Task RemoveFile(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            storageInfo.UsedStorage -= storageInfo.Files[fileName].Size;
            storageInfo.Files.Remove(fileName);
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
            StorageDirectory parentDirectory = GetDirectoryByPathList(GetPathList(path), storageInfo.Directories);

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


        private List<string> GetPathList(string path)
        {
            var pathList = path.Trim().Split("/").ToList();
            pathList.Remove(pathList.First());

            return pathList;
        }

        private StorageDirectory GetDirectoryByPathList(List<string> pathList, Dictionary<string, StorageDirectory> directories)
        {
            if (pathList.Count != 0 && directories.ContainsKey(pathList.First()))
            {
                if (pathList.Count == 1)
                {
                    return directories[pathList.First()];
                }
                else
                {
                    string tmpDirName = pathList.First();
                    pathList.Remove(pathList.First());
                    return GetDirectoryByPathList(pathList, directories[tmpDirName].Directories);
                }
            }

            throw new ArgumentException("The path doesn't exist");
        }

        public Task MoveDirectory(string oldPath, string newPath)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            //Getting old directory
            List<string> oldDirectoryPathList = GetPathList(oldPath);
            StorageDirectory movableDirectory = GetDirectoryByPathList(oldDirectoryPathList, storageInfo.Directories);

            //Getting old parent directory
            oldDirectoryPathList.Remove(oldDirectoryPathList.Last());
            StorageDirectory oldParentDirectory = GetDirectoryByPathList(oldDirectoryPathList, storageInfo.Directories);
            oldParentDirectory.Directories.Remove(movableDirectory.Name);

            //Getting new directory name
            List<string> newDirectoryPathList = GetPathList(newPath);
            string newDirName = newDirectoryPathList.Last();
            movableDirectory.Name = newDirName;

            //Getting new parent directory and adding new directory into it
            newDirectoryPathList.Remove(newDirectoryPathList.Last());
            StorageDirectory newParentDirectory = GetDirectoryByPathList(newDirectoryPathList, storageInfo.Directories);
            movableDirectory.ParentId = newParentDirectory.Name;

            if (newParentDirectory.Directories.ContainsKey(newDirName))
            {
                throw new InvalidOperationException($"The directory {newDirName} is already exists at the path '{newPath}'");
            }
            else
            {
                newParentDirectory.Directories.Add(newDirName, movableDirectory);
            }

            SerializeStorageInfoFile(storageInfo);

            return Task.CompletedTask;
        }

        public Task<StorageDirectory> GetDirectory(string path)
        {
            var pathList = GetPathList(path);
            var storageInfo = DeserializeStorageInfoFile();
            var storageDirectory = GetDirectoryByPathList(pathList, storageInfo.Directories);

            return Task.FromResult(storageDirectory);
        }

        public Task RemoveDirectory(string path)
        {
            var pathList = GetPathList(path);
            var directoryName = pathList.Last();

            pathList.Remove(pathList.First());

            var storageInfo = DeserializeStorageInfoFile();
            var parentDirectory = GetDirectoryByPathList(pathList, storageInfo.Directories);

            if (parentDirectory.Directories.ContainsKey(directoryName))
            {
                throw new ArgumentException("The path doesn't exist");
            }

            parentDirectory.Directories.Remove(directoryName);
            SerializeStorageInfoFile(storageInfo);

            return Task.CompletedTask;
        }
    }
}
