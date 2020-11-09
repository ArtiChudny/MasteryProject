using FileStorage.Models;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileStorage.Services
{
    public class StorageService
    {
        string storageInfoPath = ConfigurationManager.AppSettings["StorageInfoPath"];
        BinaryFormatter formatter = new BinaryFormatter();
        
        public StorageInfo GetStorageInfo()
        {
            return DeserializeInfoFile(formatter);
        }

        private StorageInfo DeserializeInfoFile(BinaryFormatter formatter)
        {
            using (FileStream fs = new FileStream(storageInfoPath, FileMode.OpenOrCreate))
            {
                StorageInfo storageInfo = (StorageInfo)formatter.Deserialize(fs);

                return storageInfo;
            }
        }
    }
}
