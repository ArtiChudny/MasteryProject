using System;

namespace FileStorage.ViewModels
{
    public class UserInfoViewModel
    {
        public string Login { get; set; }
        public long UsedStorage { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
