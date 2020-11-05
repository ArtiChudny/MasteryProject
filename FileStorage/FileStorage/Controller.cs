using System;
using FileStorage;
using FileStorage.Models;
using FileStorage.Services;
public static class Controller
{
    public static void ExecuteConsoleCommand(StorageCommand command)
    {
        switch (command.CommandName)
        {
            case "user":
                {
                    ConsolePrinter.PrintUserInformation(UserService.GetUserInfo());
                    break;
                }
            case "exit":
                {
                    ExitApplication();
                    break;
                }
        }
    }

    public static bool IsLoginToApp(Credentials credentials)
    {
        User user = UserService.GetUserInfo();
        if (credentials.Login == user.Login && credentials.Password == user.Password)
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
