using System.Configuration;
using FileStorage.Models;

namespace FileStorage.Services
{
    public static class UserService
    {
        public static User GetUserInfo()
        {
            return new User
            {
                Login = ConfigurationManager.AppSettings["login"],
                Password = ConfigurationManager.AppSettings["password"]
            };
        }

        public static void ExecuteCommand(StorageCommand command)
        {
            if (command.Parameters.Count!=0)
            {
                switch (command.Parameters[0])
                {
                    case "info":
                        ConsolePrinter.PrintUserInformation(UserService.GetUserInfo());
                        break;
                    default:
                        ConsolePrinter.PrintBadParameter(command.CommandName, command.Parameters[0]);
                        break;
                }
            }
            else
            {
                ConsolePrinter.PrintParameterNeeding(command.CommandName);
            }
        }
    }
}
