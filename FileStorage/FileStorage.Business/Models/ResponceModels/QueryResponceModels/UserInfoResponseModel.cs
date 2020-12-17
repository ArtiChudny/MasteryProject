using System;

namespace FileStorage.BLL.Models.ResponceModels.QueryResponceModels
{
    public class UserInfoResponseModel
    {
        public string Login { get; set; }
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
