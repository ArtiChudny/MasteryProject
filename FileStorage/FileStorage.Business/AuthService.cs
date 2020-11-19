using FileStorage.BLL.Interfaces;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;

namespace FileStorage.BLL
{
    public class AuthService : IAuthService
    {
        private IUserRepository userProvider;
        public AuthService(IUserRepository userProvider)
        {
            this.userProvider = userProvider;
        }

        public bool IsAuthenticated(string login, string password)
        {
            User user = userProvider.GetUser();
            if (login == user.Login && password == user.Password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
