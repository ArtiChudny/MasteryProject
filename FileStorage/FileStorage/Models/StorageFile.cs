using System;

namespace FileStorage.Models
{
    [Serializable]
    public class StorageFile
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public int DownloadsNumber { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
