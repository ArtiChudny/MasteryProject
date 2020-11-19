using FileStorage.BLL.Interfaces;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;

namespace FileStorage.BLL
{
    public class UserService : IUserService
    {
        private IUserRepository userProvider;

        public UserService(IUserRepository userProvider)
        {
            this.userProvider = userProvider;
        }

        public User GetUser()
        {
           return userProvider.GetUser();
        }
    }
}
