using MediatR;

namespace FileStorage.BLL.Queries
{
    public class IsAuthenticatedQuery : IRequest<bool>
    {
        public string Login { get; }
        public string Password { get; }

        public IsAuthenticatedQuery(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
