using System;
using System.Linq;
using FileStorage.Models;

static class StorageCommand
{
    public static bool LoginToApp(string[] args)
    {
        if (args.Count() == 4 && (args[0] == "--l" || args[2] == "--l") && (args[0] == "--p" || args[2] == "--p"))
        {
            if (args[0] == "--l" && args[2] == "--p")
            {
                return CheckAuthentification(args[1], args[3]);
            }
            else
            {
                return CheckAuthentification(args[3], args[1]);
            }
        }
        else
        {
            Console.WriteLine("Uncorrect initial parameters. Use --l for login and --p for password.");
            return false;
        }
    }

    public static bool CheckAuthentification(string login, string password)
    {
        if (login == StorageInfo.login && password == StorageInfo.password)
        {
            return true;
        }
        else
        {
            Console.WriteLine("Incorrect login or password");
            return false;
        }
    }
}
