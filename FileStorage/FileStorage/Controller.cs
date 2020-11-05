using System;
using FileStorage;
using FileStorage.Enums;
using FileStorage.Models;
using FileStorage.Services;

static class Controller
{
    public static void ExecuteConsoleCommand(StorageCommands consoleCommand)
    {
        switch (consoleCommand)
        {
            case StorageCommands.user:
                {
                    ConsolePrinter.PrintUserInformation(UserService.GetUserInfo());
                    break;
                }
            case StorageCommands.exit:
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
