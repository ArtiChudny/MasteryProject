using System;
using System.Collections.Generic;

namespace FileStorage.Models
{
    [Serializable]
    public class SerializableStorageInfo
    {
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
        public List<KeyValue> Files { get; set; }

        public SerializableStorageInfo()
        {
            Files = new List<KeyValue>();
        }
    }

    [Serializable]
    public class KeyValue
    {
        public string Key { get; set; }
        public StorageFile Value { get; set; }
    }
}
