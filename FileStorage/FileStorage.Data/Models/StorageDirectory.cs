using System;
using System.Collections.Generic;

namespace FileStorage.DAL.Models
{
    [Serializable]
    public class StorageDirectory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public List<StorageFile> Files { get; set; }
        public List<StorageDirectory> Directories { get; set; }

        public int? ParentId { get; set; }
        public StorageDirectory ParentDirectory { get; set; }

        public int UserId { get; set; }

        public StorageDirectory()
        {
            Files = new List<StorageFile>();
            Directories = new List<StorageDirectory>();
        }
    }
}
