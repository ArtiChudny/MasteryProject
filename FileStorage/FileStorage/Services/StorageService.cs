﻿using FileStorage.Models;
using System;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileStorage.Services
{
    public class StorageService
    {
        string storageInfoPath = ConfigurationManager.AppSettings["StorageInfoPath"];
        long maxStorage = Convert.ToInt64(ConfigurationManager.AppSettings["MaxStorage"]);
        long maxFileSize = Convert.ToInt64(ConfigurationManager.AppSettings["MaxFileSize"]);

        BinaryFormatter binaryFormatter;

        public StorageService()
        {
            binaryFormatter = new BinaryFormatter();
        }

        public StorageInfo GetStorageInfo()
        {
            return DeserializeStorageInfoFile();
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

        public StorageFile CreateNewStorageFile(string fileName, long fileSize, byte[] hash, DateTime creationDate)
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

        public byte[] GetStorageFileHash(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            return storageInfo.Files[fileName].Hash;
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

        public Guid GetFileGuid(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            return storageInfo.Files[fileName].Id;
        }
    }
}
