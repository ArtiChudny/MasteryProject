namespace FileStorage.DAL
{
    public class CurrentUser
    {
        private int userId;
        private string userLogin;

        public int Id { get { return userId; } }
        public string Login { get { return userLogin; } }

        public void InitialiseUser(int id, string login)
        {
            userId = id;
            userLogin = login;
        }
    }
}
