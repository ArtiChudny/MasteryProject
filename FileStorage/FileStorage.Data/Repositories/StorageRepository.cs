using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileStorage.DAL.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private string storageInfoPath = ConfigurationManager.AppSettings["StorageInfoPath"];
        private long maxStorage = Convert.ToInt64(ConfigurationManager.AppSettings["MaxStorage"]);
        private long maxFileSize = Convert.ToInt64(ConfigurationManager.AppSettings["MaxFileSize"]);
        private BinaryFormatter binaryFormatter;

        public StorageRepository()
        {
            binaryFormatter = new BinaryFormatter();
        }

        public StorageFile CreateFile(string fileName, long fileSize, byte[] hash, DateTime creationDate)
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

            return storageFile;
        }

        public byte[] GetFileHash(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            return storageInfo.Files[fileName].Hash;
        }

        public StorageFile GetFileInfo(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            if (storageInfo.Files.ContainsKey(fileName))
            {
                return storageInfo.Files[fileName];
            }
            else
            {
                return null;
            }
        }

        public StorageInfo GetStorageInfo()
        {
            return DeserializeStorageInfoFile();
        }

        public void IncreaseDownloadsCounter(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            storageInfo.Files[fileName].DownloadsNumber++;
            SerializeStorageInfoFile(storageInfo);
        }

        public void InitializeStorage()
        {
            string storagePath = Path.GetDirectoryName(storageInfoPath);

            if (!Directory.Exists(storagePath))
            {
                throw new ApplicationException($"Missing path '{Path.GetFullPath(storagePath)}'");
            }

            if (!File.Exists(storageInfoPath))
            {
                StorageInfo storageInfo = new StorageInfo();
                SerializeStorageInfoFile(storageInfo);
            }
        }

        public bool IsEnoughStorageSpace(long fileSize)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            return (storageInfo.UsedStorage + fileSize) < maxStorage;
        }

        public bool IsFileSizeLessThanMaxSize(long fileSize)
        {
            return fileSize < maxFileSize;
        }

        public void MoveFile(string oldFileName, string newFileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageFile storageFile = storageInfo.Files[oldFileName];
            storageInfo.Files.Remove(oldFileName);
            storageFile.Extension = Path.GetExtension(newFileName);
            storageInfo.Files.Add(newFileName, storageFile);
            SerializeStorageInfoFile(storageInfo);
        }

        public void RemoveFile(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            storageInfo.UsedStorage -= storageInfo.Files[fileName].Size;
            storageInfo.Files.Remove(fileName);
            SerializeStorageInfoFile(storageInfo);
        }

        private void SerializeStorageInfoFile(StorageInfo storageInfo)
        {
            using (FileStream fileStream = new FileStream(storageInfoPath, FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(fileStream, storageInfo);
            }
        }

        private StorageInfo DeserializeStorageInfoFile()
        {
            using (FileStream fileStream = new FileStream(storageInfoPath, FileMode.OpenOrCreate))
            {
                StorageInfo storageInfo = (StorageInfo)binaryFormatter.Deserialize(fileStream);

                return storageInfo;
            }
        }

        public void CreateDirectory(string path, string directoryName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageDirectory parentDirectory = GetDirectoryByPath(storageInfo, GetDirectoriesList(path));

            if (parentDirectory.Directories.ContainsKey(directoryName))
            {
                throw new ApplicationException($"The directory {directoryName} is already exists at the path '{path}'");
            }

            parentDirectory.Directories.Add(directoryName, new StorageDirectory()
            {
                Name = directoryName,
                ParentId = parentDirectory.Name
            });

            SerializeStorageInfoFile(storageInfo);
        }


        private List<string> GetDirectoriesList(string path)
        {
            return path.Replace("/", " ").Trim().Split(" ").ToList();
        }

        private StorageDirectory GetDirectoryByPath(StorageInfo storageInfo, List<string> directoriesList)
        {
            StorageDirectory storageDirectory = null;

            for (int arrayIndex = 0; arrayIndex < directoriesList.Count; arrayIndex++)
            {
                if (arrayIndex == 0 && storageInfo.InitialDirectory.Name == directoriesList[0])
                {
                    storageDirectory = storageInfo.InitialDirectory;
                }
                //TODO: Possible NullReferenceException (storageDirectory)
                else if (arrayIndex != 0 && storageDirectory.Directories.ContainsKey(directoriesList[arrayIndex]))
                {
                    storageDirectory = storageDirectory.Directories[directoriesList[arrayIndex]];
                }
                else
                {
                    throw new ApplicationException($"The path doesn't exist");
                }
            }

            return storageDirectory;
        }

        public void MoveDirectory(string oldPath, string newPath)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            //Getting old directory
            List<string> oldDirectoryPath = GetDirectoriesList(oldPath);
            StorageDirectory movableDirectory = GetDirectoryByPath(storageInfo, oldDirectoryPath);
            //Getting old parent directory
            oldDirectoryPath.Remove(oldDirectoryPath.Last());
            StorageDirectory oldParentDirectory = GetDirectoryByPath(storageInfo, oldDirectoryPath);
            oldParentDirectory.Directories.Remove(movableDirectory.Name);
            //Getting new directory name
            List<string> newDirectoryPath = GetDirectoriesList(newPath);
            string newDirName = newDirectoryPath.Last();
            movableDirectory.Name = newDirName;
            //Getting new parent directory and adding new directory into it
            newDirectoryPath.Remove(newDirectoryPath.Last());
            StorageDirectory newParentDirectory = GetDirectoryByPath(storageInfo, newDirectoryPath);
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
        }
    }
}
