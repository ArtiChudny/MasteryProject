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
        BinaryFormatter formatter = new BinaryFormatter();

        public StorageInfo GetStorageInfo()
        {
            return DeserializeInfoFile();
        }

        public void AddNewFile(StorageFile storageFile)
        {
            StorageInfo storageInfo = DeserializeInfoFile();
            storageInfo.UsedStorage += storageFile.Size;
            storageInfo.StorageFiles.Add(storageFile);
            SerializeStorageInfoFile(storageInfo);
        }

        private void SerializeStorageInfoFile(StorageInfo storageInfo)
        {
            using (FileStream fs = new FileStream(storageInfoPath, FileMode.Open))
            {
                formatter.Serialize(fs, storageInfo);
            }
        }

        private StorageInfo DeserializeInfoFile()
        {
            using (FileStream fs = new FileStream(storageInfoPath, FileMode.OpenOrCreate))
            {
                StorageInfo storageInfo = (StorageInfo)formatter.Deserialize(fs);

                return storageInfo;
            }
        }

        internal void IncreaseDownloadsCount(string fileName)
        {
            StorageInfo storageInfo = DeserializeInfoFile();
            storageInfo.StorageFiles.Where(f => f.FileName == fileName).First().DownloadsNumber++;
            SerializeStorageInfoFile(storageInfo);
        }
    }
}
