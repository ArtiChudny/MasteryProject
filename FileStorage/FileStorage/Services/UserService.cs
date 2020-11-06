using System.Configuration;
using FileStorage.Models;

namespace FileStorage.Services
{
    public static class UserService
    {
        public static User GetUser()
        {
            return new User
            {
                Login = ConfigurationManager.AppSettings["login"],
                Password = ConfigurationManager.AppSettings["password"]
            };
        }
    }
}
