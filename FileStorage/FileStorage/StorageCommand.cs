using System;
using System.Linq;
using FileStorage;
using FileStorage.Models;
using FileStorage.Services;

static class StorageCommand
{
    public static void ExecuteConsoleCommand(string consoleCommand)
    {
        string[] commandArray = consoleCommand.Split(" ");
        switch (commandArray[0])
        {
            case "user info":
                {
                    ConsolePrinter.PrintUserInformation(UserService.GetUserInfo());
                    break;
                }
            case "exit":
                {
                    ExitApplication();
                    break;
                }

            default:
                {
                    ConsolePrinter.PrintWrongCommand(commandArray[0]);
                    break;
                }
        }
    }

    public static bool IsLoginToApp(string login, string password)
    {
        User user = UserService.GetUserInfo();
        if (login == user.Login && password == user.Password)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void ExitApplication()
    {
        Environment.Exit(-1);
    }
}
