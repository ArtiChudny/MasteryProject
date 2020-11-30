using MediatR;

namespace FileStorage.BLL.Queries
{
    public class IsAuthenticatedQuery : IRequest<bool>
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public IsAuthenticatedQuery(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
