using System;
using FileStorage.Enums;
using FileStorage.Models;

namespace FileStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var flags = ConsoleFlagParser.Parse(args);
                if (flags.ContainsKey(StorageFlags.Login) && flags.ContainsKey(StorageFlags.Password))
                {
                    Credentials credentials = new Credentials(flags[StorageFlags.Login], flags[StorageFlags.Password]);
                    if (Controller.IsLoginToApp(credentials))
                    {
                        ConsolePrinter.PrintAuthenticationSuccessful();
                        while (true)
                        {
                            ConsolePrinter.PrintСommandWaitingIcon();
                            string currentCommand = Console.ReadLine().ToLower();
                            try
                            {
                               Controller.ExecuteConsoleCommand(ConsoleCommandParser.Parse(currentCommand));
                            }
                            catch (ApplicationException ex)
                            {
                                ConsolePrinter.PrintErrorMessage(ex.Message);
                            }
                            catch (Exception ex)
                            {
                                ConsolePrinter.PrintErrorMessage(ex.Message);
                                Controller.ExitApplication();
                            }
                        }
                    }
                    else
                    {
                        ConsolePrinter.PrintAuthenticationFailed();
                        Controller.ExitApplication();
                    }
                }
                else
                {
                    ConsolePrinter.PrintBadInitialParameters();
                }
            }
            catch (ApplicationException ex)
            {
                ConsolePrinter.PrintErrorMessage(ex.Message);
                Controller.ExitApplication();
            }
            catch (Exception ex)
            {
                ConsolePrinter.PrintErrorMessage(ex.Message);
                Controller.ExitApplication();
            }
        }
    }
}
