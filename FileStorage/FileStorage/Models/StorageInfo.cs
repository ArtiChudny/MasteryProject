using System;
using System.Collections.Generic;

namespace FileStorage.Models
{
    [Serializable]
    public class StorageInfo
    {
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
        public IList<StorageFile> Files { get; set; }

        public StorageInfo()
        {
            UsedStorage = 0;
            CreationDate = DateTime.Today;
            Files = new List<StorageFile>();
        }
    }
}
