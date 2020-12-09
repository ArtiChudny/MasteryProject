using System;
using System.Collections.Generic;

namespace FileStorage.DAL.Models
{
    [Serializable]
    public class SerializableStorageInfo
    {
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
        public List<SerializableStorageDirectoryKeyValuePair> Directories { get; set; }

        public SerializableStorageInfo()
        {
            Directories = new List<SerializableStorageDirectoryKeyValuePair>();
        }
    }

    [Serializable]
    public class SerializableStorageFileKeyValuePair
    {
        public string Key { get; set; }
        public StorageFile Value { get; set; }
    }

    [Serializable]
    public class SerializableStorageDirectoryKeyValuePair
    {
        public string Key { get; set; }
        public SerializableStorageDirectory Value { get; set; }
    }

    [Serializable]
    public class SerializableStorageDirectory
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public string ParentId { get; set; }
        public List<SerializableStorageFileKeyValuePair> Files { get; set; }
        public List<SerializableStorageDirectoryKeyValuePair> Directories { get; set; }

        public SerializableStorageDirectory()
        {
            Files = new List<SerializableStorageFileKeyValuePair>();
            Directories = new List<SerializableStorageDirectoryKeyValuePair>();
        }
    }
}
