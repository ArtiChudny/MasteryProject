using System;

namespace FileStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            if (ConsoleCommandParser.IsInitialParametersCorrect(args))
            {
                string login = string.Empty;
                string password = string.Empty;
                
                ConsoleCommandParser.GetCredentialsFromInitialParameters(args, ref login, ref password);

                if (StorageCommand.IsLoginToApp(login, password))
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
