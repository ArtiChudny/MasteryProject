using System;
using FileStorage.Models;

namespace FileStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            if (ConsoleCommandParser.IsInitialParametersCorrect(args))
            {
                Credentials credentials = ConsoleCommandParser.GetCredentialsFromInitialParameters(args);
                if (StorageCommand.IsLoginToApp(credentials))
                {
                    ConsolePrinter.PrintAuthenticationSuccessful();
                    while (true)
                    {
                        ConsolePrinter.PrintСommandWaitingIcon();
                        string currentCommand = Console.ReadLine().ToLower();
                        StorageCommand.ExecuteConsoleCommand(currentCommand);
                    }
                }
                else
                {
                    ConsolePrinter.PrintAuthenticationFailed();
                    StorageCommand.ExitApplication();
                }
            }
            else
            {
                ConsolePrinter.PrintBadInitialParameters();
                StorageCommand.ExitApplication();
            }
        }
    }
}
