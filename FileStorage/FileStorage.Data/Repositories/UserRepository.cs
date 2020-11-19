using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using System.Configuration;

namespace FileStorage.DAL.Repositories
{
    public class UserRepository : IUserRepository
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
