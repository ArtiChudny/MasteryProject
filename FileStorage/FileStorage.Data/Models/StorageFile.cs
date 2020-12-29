using System;

namespace FileStorage.DAL.Models
{
    [Serializable]
    public class StorageFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Guid GuidName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public int DownloadsNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] Hash { get; set; }
        
        public int DirectoryId { get; set; }
        public StorageDirectory ParentDirectory { get; set; }
    }
}
