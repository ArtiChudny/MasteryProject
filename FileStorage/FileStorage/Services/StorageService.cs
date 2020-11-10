using FileStorage.Models;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileStorage.Services
{
    public class StorageService
    {
        string storageInfoPath = ConfigurationManager.AppSettings["StorageInfoPath"];
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

            if ((storageInfo.UsedStorage + fileSize) > storageInfo.MaxStorage)
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

            if (fileSize < storageInfo.MaxFileSize)
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
            storageInfo.StorageFiles.Add(storageFile);
            SerializeStorageInfoFile(storageInfo);
        }

        public void MoveFile(string oldFileName, string newFileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageFile storageFile = storageInfo.StorageFiles.Where(f => f.FileName == oldFileName).First();
            storageFile.FileName = newFileName;
            storageFile.Extension = Path.GetExtension(newFileName);
            SerializeStorageInfoFile(storageInfo);
        }

        public void RemoveFile(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageFile storageFile = storageInfo.StorageFiles.Where(f => f.FileName == fileName).First();
            storageInfo.UsedStorage -= storageFile.Size;
            storageInfo.StorageFiles.Remove(storageFile);
            SerializeStorageInfoFile(storageInfo);
        }

        public StorageFile GetFileInfo(string fileName)
        {
            StorageInfo storageInfo = DeserializeStorageInfoFile();
            StorageFile storageFile = storageInfo.StorageFiles.Where(f => f.FileName == fileName).First();
            return storageFile;
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

        public bool IsStorageInfoFileExists()
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
