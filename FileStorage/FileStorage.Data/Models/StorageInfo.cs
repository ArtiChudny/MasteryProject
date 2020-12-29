using System;
using System.Collections.Generic;

namespace FileStorage.DAL.Models
{
    [Serializable]
    public class StorageInfo
    {
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
        public StorageDirectory InitialDirectory { get; set; }
    }
}
