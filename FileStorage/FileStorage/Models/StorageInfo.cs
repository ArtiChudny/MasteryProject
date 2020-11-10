using System;
using System.Collections.Generic;

namespace FileStorage.Models
{
    [Serializable]
    public class StorageInfo
    {
        public long MaxStorage { get; set; }
        public long MaxFileSize { get; set; }
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
        public List<StorageFile> StorageFiles { get; set; }

        public StorageInfo()
        {
            MaxStorage = 5368709120; //5GB
            MaxFileSize = 2147483648; //2GB
            UsedStorage = 0;
            CreationDate = DateTime.Today;
            StorageFiles = new List<StorageFile>();
        }
    }
}
