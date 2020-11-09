using System;
using System.Collections.Generic;

namespace FileStorage.Models
{
    [Serializable]
    public class StorageInfo
    {
        public int MaxStorage { get; set; }
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
        public List<StorageFile> StorageFiles { get; set; }

        public StorageInfo()
        {
            MaxStorage = 536870912; //512MB int bytes
            UsedStorage = 0;
            CreationDate = DateTime.Today;
            StorageFiles = new List<StorageFile>();
        }
    }
}
