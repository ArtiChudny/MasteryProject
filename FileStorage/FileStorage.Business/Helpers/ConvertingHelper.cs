using FileStorage.DAL.Models;

namespace FileStorage.BLL.Helpers
{
    public class ConvertingHelper
    {
        public static SerializableStorageInfo GetSerializableStorageInfo(StorageInfo storageInfo)
        {
            SerializableStorageInfo serializableStorageInfo = new SerializableStorageInfo
            {
                CreationDate = storageInfo.CreationDate,
                UsedStorage = storageInfo.UsedStorage
            };

            foreach (var file in storageInfo.Files)
            {
                KeyValue pair = new KeyValue
                {
                    Key = file.Key,
                    Value = file.Value
                };

                serializableStorageInfo.Files.Add(pair);
            }

            return serializableStorageInfo;
        }
    }
}
