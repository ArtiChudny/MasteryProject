using System;
using System.Collections.Generic;

namespace FileStorage.DAL.Models
{
    [Serializable]
    public class StorageInfo
    {
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
        public Dictionary<string, StorageDirectory> Directories { get; set; }

        public StorageInfo()
        {
            CreationDate = DateTime.Today;
            Directories = new Dictionary<string, StorageDirectory>();
            Directories.Add("root", new StorageDirectory() { Name = "root" });
        }
    }
}
