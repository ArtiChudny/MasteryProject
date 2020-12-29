using System;
using System.Collections.Generic;

namespace FileStorage.DAL.Models
{
    [Serializable]
    public class SerializableStorageInfo
    {
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
        public SerializableStorageDirectory InitialDirectory { get; set; }
    }

    [Serializable]
    public class SerializableStorageFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid GuidName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public int DownloadsNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] Hash { get; set; }
        public string Path { get; set; }

        public int StorageDirectoryId { get; set; }
    }

    [Serializable]
    public class SerializableStorageDirectory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public string Path { get; set; }
        public List<SerializableStorageFile> Files { get; set; }
        public List<SerializableStorageDirectory> Directories { get; set; }

        public int? ParentId { get; set; }

        public SerializableStorageDirectory()
        {
            Files = new List<SerializableStorageFile>();
            Directories = new List<SerializableStorageDirectory>();
        }
    }
}
