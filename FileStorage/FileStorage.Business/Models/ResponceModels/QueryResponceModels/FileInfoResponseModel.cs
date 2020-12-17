using System;

namespace FileStorage.BLL.Models.ResponceModels.QueryResponceModels
{
    public class FileInfoResponseModel
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long FileSize { get; set; }
        public DateTime CreationDate { get; set; }
        public string Login { get; set; }
        public int DownloadsNumber { get; set; }
    }
}
