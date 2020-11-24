using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using System;
using System.Configuration;
using System.IO;
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
            StorageDirectory parentDirectory = GetDirectoryByPath(storageInfo, path);

            if (parentDirectory.SubDirectory != null)
            {
                throw new ApplicationException($"There is directory already exists at the path '{path}'");
            }

            parentDirectory.SubDirectory = new StorageDirectory()
            {
                Name = directoryName,
                ParentId = parentDirectory.Name
            };

            SerializeStorageInfoFile(storageInfo);
        }

        private StorageDirectory GetDirectoryByPath(StorageInfo storageInfo, string path)
        {
            string[] directoriesArray = path.Replace("/", " ").Trim().Split(" ");
            StorageDirectory storageDirectory = null;

            for (int arrayIndex = 0; arrayIndex < directoriesArray.Length; arrayIndex++)
            {
                if (arrayIndex == 0 && storageInfo.InitialDirectory.Name == directoriesArray[0])
                {
                    storageDirectory = storageInfo.InitialDirectory;
                }
                else if (arrayIndex != 0 && storageDirectory.SubDirectory.Name == directoriesArray[arrayIndex])
                {
                    storageDirectory = storageDirectory.SubDirectory;
                }
                else
                {
                    throw new ApplicationException($"The path '{path}' doesn't exist");
                }
            }

            return storageDirectory;
        }
    }
}
