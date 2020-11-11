using FileStorage.Models;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
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

            if ((storageInfo.UsedStorage + fileSize) > maxStorage)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsFileSizeLessThanMaxSize(long fileSize)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();

            if (fileSize < maxFileSize)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddFileToStorage(StorageFile storageFile)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            storageInfo.UsedStorage += storageFile.Size;
            storageInfo.Files.Add(storageFile);
            SerializeStorageInfoFile(storageInfo);
        }

        public void MoveFile(string oldFileName, string newFileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageFile storageFile = storageInfo.Files.Where(f => f.FileName == oldFileName).First();
            storageFile.FileName = newFileName;
            storageFile.Extension = Path.GetExtension(newFileName);
            SerializeStorageInfoFile(storageInfo);
        }

        public void RemoveFile(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageFile storageFile = storageInfo.Files.Where(f => f.FileName == fileName).First();
            storageInfo.UsedStorage -= storageFile.Size;
            storageInfo.Files.Remove(storageFile);
            SerializeStorageInfoFile(storageInfo);
        }

        public StorageFile GetFileInfo(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageFile storageFile = storageInfo.Files.Where(f => f.FileName == fileName).First();
            return storageFile;
        }

        public byte[] GetStorageFileMD5Hash(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageFile storageFile = storageInfo.Files.Where(f => f.FileName == fileName).First();
            return storageFile.Md5Hash;
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
            storageInfo.Files.Where(f => f.FileName == fileName).First().DownloadsNumber++;
            SerializeStorageInfoFile(storageInfo);
        }

        public void InitializeStorage()
        {
            if (!File.Exists(storageInfoPath))
            {
                StorageInfo storageInfo = new StorageInfo();
                SerializeStorageInfoFile(storageInfo);
            }
        }
    }
}
