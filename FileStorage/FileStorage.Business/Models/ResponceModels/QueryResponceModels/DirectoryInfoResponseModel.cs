using System;

namespace FileStorage.BLL.Models.ResponceModels.QueryResponceModels
{
    public class DirectoryInfoResponseModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long Size { get; set; }
        public string Login { get; set; }
    }
}
