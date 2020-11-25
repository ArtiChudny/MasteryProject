using System;
using System.Collections.Generic;

namespace FileStorage.DAL.Models
{
    [Serializable]
    public class StorageDirectory
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public string ParentId { get; set; }
        public Dictionary<string, StorageFile> Files { get; set; }
        public Dictionary<string, StorageDirectory> Directories { get; set; }

        public StorageDirectory()
        {
            Files = new Dictionary<string, StorageFile>();
            Directories = new Dictionary<string, StorageDirectory>();
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
        }
    }
}
