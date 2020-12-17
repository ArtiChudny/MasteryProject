using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using System.Configuration;
using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User> GetUser()
        {
            var currentUser = new User
            {
                Login = ConfigurationManager.AppSettings["login"],
                Password = ConfigurationManager.AppSettings["password"]
            };

            return Task.FromResult(currentUser);
        }
    }
}
