using System.Configuration;
using FileStorage.Models;

namespace FileStorage.Services
{
    public class UserService
    {
        public User GetUser()
        {
            return new User
            {
                Login = ConfigurationManager.AppSettings["login"],
                Password = ConfigurationManager.AppSettings["password"]
            };
        }
    }
}
