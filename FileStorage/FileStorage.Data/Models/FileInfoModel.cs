using System;

namespace FileStorage.DAL.Models
{
    public class FileInfoModel
    {
        public long Size { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] Hash { get; set; }
    }
}
