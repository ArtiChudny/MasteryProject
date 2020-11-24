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
        public StorageDirectory SubDirectory { get; set; }

        public StorageDirectory()
        {
            Files = new Dictionary<string, StorageFile>();
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Today;
        }
    }
}
