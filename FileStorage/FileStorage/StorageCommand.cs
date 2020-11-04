using System;
using System.Linq;
using FileStorage;
using FileStorage.Models;

static class StorageCommand
{
    public static void ExecuteConsoleCommand(string consoleCommand)
    {
        string[] commandArray = consoleCommand.Split(" ");
        switch (commandArray[0])
        {
            case "user":
                {
                    ExecuteUserCommand(commandArray);
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

    private static void ExecuteUserCommand(string[] commandArray)
    {
        if (commandArray.Count() > 1)
        {
            if (commandArray[1] == "info")
            {
                ConsolePrinter.PrintStorageInformation();
            }
            else
            {
                ConsolePrinter.PrintBadParameter(commandArray[0], commandArray[1]);
            }
        }
        else
        {
            ConsolePrinter.PrintParameterNeeding(commandArray[0]);
        }
    }

    public static bool LoginToApp(string[] args)
    {
        if (args.Count() == 4 && (args[0] == "--l" || args[2] == "--l") && (args[0] == "--p" || args[2] == "--p"))
        {
            if (args[0] == "--l" && args[2] == "--p")
            {
                return CheckAuthentication(args[1], args[3]);
            }
            else
            {
                return CheckAuthentication(args[3], args[1]);
            }
        }
        else
        {
            return false;
        }
    }

    public static bool CheckAuthentication(string login, string password)
    {
        if (login == StorageInfo.login && password == StorageInfo.password)
        {
            return true;
        }
        else
        {
            ConsolePrinter.PrintAuthenticationFailed();
            return false;
        }
    }
    public static void ExitApplication()
    {
        Environment.Exit(-1);
    }
}
