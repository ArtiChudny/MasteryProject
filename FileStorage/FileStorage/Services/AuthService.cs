using FileStorage.Models;

namespace FileStorage.Services
{
    public class AuthService
    {
        private UserService userService;

        public AuthService()
        {
            userService = new UserService();
        }

        public bool IsAuthenticated(Credentials credentials)
        {
            User user = userService.GetUser();
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
