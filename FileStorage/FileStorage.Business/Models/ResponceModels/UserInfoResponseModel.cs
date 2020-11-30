using System;

namespace FileStorage.BLL.Models.ResponceModels
{
    public class UserInfoResponseModel
    {
        public string Login { get; set; }
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
