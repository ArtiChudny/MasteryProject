namespace FileStorage.BLL.Interfaces
{
    public interface IAuthService
    {
        bool IsAuthenticated(string login, string password);
    }
}
