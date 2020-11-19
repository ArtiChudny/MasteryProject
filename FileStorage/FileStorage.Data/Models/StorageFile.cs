using System;

namespace FileStorage.DAL.Models
{
    [Serializable]
    public class StorageFile
    {
        public Guid Id { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public int DownloadsNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] Hash { get; set; }
    }
}
