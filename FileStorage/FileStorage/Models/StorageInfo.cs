using System.Configuration;

namespace FileStorage.Models
{
    static class StorageInfo
    {
        public static readonly string login;
        public static readonly string password;
        public static readonly string maxStorage;
        public static readonly string creationDate;

        static StorageInfo()
        {
            login = ConfigurationManager.AppSettings["login"];
            password = ConfigurationManager.AppSettings["password"];
            maxStorage = ConfigurationManager.AppSettings["maxStorage"];
            creationDate = ConfigurationManager.AppSettings["creationDate"];
        }
    }
}
