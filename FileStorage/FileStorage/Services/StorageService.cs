﻿using FileStorage.Models;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileStorage.Services
{
    public class StorageService
    {
        string storageInfoPath = ConfigurationManager.AppSettings["StorageInfoPath"];
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        public StorageInfo GetStorageInfo()
        {
            return DeserializeStorageInfoFile();
        }

        public void AddFileToStorage(StorageFile storageFile)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            storageInfo.UsedStorage += storageFile.Size;
            storageInfo.StorageFiles.Add(storageFile);
            SerializeStorageInfoFile(storageInfo);
        }

        public void MoveFile(string oldFileName, string newFileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            storageInfo.StorageFiles.Where(f => f.FileName == oldFileName).First().FileName = newFileName;
            SerializeStorageInfoFile(storageInfo);
            //should to do changing extension here if it will change on command
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

        internal void IncreaseDownloadsCounter(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            storageInfo.StorageFiles.Where(f => f.FileName == fileName).First().DownloadsNumber++;
            SerializeStorageInfoFile(storageInfo);
        }

        public bool IfStorageInfoFileExists()
        {
            if (File.Exists(storageInfoPath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateStorageInfoFile()
        { 
            StorageInfo storageInfo = new StorageInfo();
            SerializeStorageInfoFile(storageInfo);
        }
    }
}
