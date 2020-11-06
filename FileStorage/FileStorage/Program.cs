using System;
using System.Collections.Generic;
using FileStorage.Enums;
using FileStorage.Models;
using FileStorage.Services;

namespace FileStorage
{
    class Program
    {
        static ConsolePrinter consolePrinter = new ConsolePrinter();
        static void Main(string[] args)
        {
            Controller controller = new Controller(consolePrinter);
            AuthService authService = new AuthService();
            try
            {
                Dictionary<StorageFlags, string> flags = ConsoleFlagParser.Parse(args);
                Credentials credentials = GetCredentials(flags);
                if (!authService.IsAuthenticated(credentials))
                {
                    throw new ApplicationException("Incorrect login or password");
                }
                consolePrinter.PrintAuthenticationSuccessful();

                while (true)
                {
                    try
                    {
                        StorageCommand command = GetCommand();
                        controller.ExecuteConsoleCommand(command);
                    }
                    catch (Exception ex)
                    {
                        consolePrinter.PrintErrorMessage(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                consolePrinter.PrintErrorMessage(ex.Message);
            }
        }

        private static Credentials GetCredentials(Dictionary<StorageFlags, string> flags)
        {
            if (IsContainLoginPassword(flags))
            {
                return new Credentials(flags[StorageFlags.Login], flags[StorageFlags.Login]);
            }
            else
            {
                throw new ApplicationException("You have to enter your login and password. Use --l for login and --p for password.");
            }
        }

        private static bool IsContainLoginPassword(Dictionary<StorageFlags, string> flags)
        {
            if (flags.ContainsKey(StorageFlags.Login) && flags.ContainsKey(StorageFlags.Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static StorageCommand GetCommand()
        {
            consolePrinter.PrintСommandWaitingIcon();
            string currentCommand = Console.ReadLine().ToLower();

            return ConsoleCommandParser.Parse(currentCommand);
        }
    }
}
