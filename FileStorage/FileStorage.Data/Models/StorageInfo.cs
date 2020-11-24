using System;
using System.Collections.Generic;

namespace FileStorage.DAL.Models
{
    [Serializable]
    public class StorageInfo
    {
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
        public Dictionary<string, StorageFile> Files { get; set; }
        public StorageDirectory InitialDirectory { get; set; }

        public StorageInfo()
        {
            CreationDate = DateTime.Today;
            Files = new Dictionary<string, StorageFile>();
            InitialDirectory = new StorageDirectory()
            {
                Name = "root"
            };
        }
    }
}
