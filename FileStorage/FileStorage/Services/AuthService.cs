using FileStorage.Models;

namespace FileStorage.Services
{
    public class AuthService
    {
        public bool IsAuthenticated(Credentials credentials)
        {
            User user = UserService.GetUser();
            if (credentials.Login == user.Login && credentials.Password == user.Password)
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
