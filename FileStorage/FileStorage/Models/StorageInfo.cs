using System;
using System.Collections.Generic;

namespace FileStorage.Models
{
    [Serializable]
    public class StorageInfo
    {
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
        public IDictionary<string, StorageFile> Files { get; set; }

        public StorageInfo()
        {
            UsedStorage = 0;
            CreationDate = DateTime.Today;
            Files = new Dictionary<string, StorageFile>();
        }
    }
}
