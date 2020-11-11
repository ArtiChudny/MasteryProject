using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using FileStorage.Enums;
using FileStorage.Models;
using FileStorage.Services;

namespace FileStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthService authService = new AuthService();
            ConsolePrinter consolePrinter = new ConsolePrinter();
            StorageFileService storageFileService = new StorageFileService();
            Controller controller = new Controller(consolePrinter, storageFileService);
            ConsoleCommandParser consoleCommandParser = new ConsoleCommandParser();

            try
            {
                InitializeStorage(storageFileService);

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
                        StorageCommand command = GetCommand(consolePrinter, consoleCommandParser);
                        if (command.CommandType == StorageCommands.Exit)
                        {
                            consolePrinter.PrintExitMessage();
                            break;
                        }
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
                Environment.Exit(-1);
            }
        }

        private static Credentials GetCredentials(Dictionary<StorageFlags, string> flags)
        {
            if (IsContainLoginPassword(flags))
            {
                return new Credentials(flags[StorageFlags.Login], flags[StorageFlags.Password]);
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

        private static StorageCommand GetCommand(ConsolePrinter consolePrinter, ConsoleCommandParser consoleCommandParser)
        {
            consolePrinter.PrintСommandWaitingIcon();
            string rowCommand = Console.ReadLine().ToLower().Trim();

            if (string.IsNullOrWhiteSpace(rowCommand))
            {
                throw new ApplicationException("You have not entered a command.");
            }

            return consoleCommandParser.Parse(rowCommand);
        }

        private static void InitializeStorage(StorageFileService storageFileService)
        {
            string storageFilesPath = ConfigurationManager.AppSettings["StoragePath"];
            string storagePath = Path.GetDirectoryName(ConfigurationManager.AppSettings["StorageInfoPath"]);

            if (!Directory.Exists(storagePath) || !Directory.Exists(storageFilesPath))
            {
                throw new ApplicationException($"Missing storage directory '{Path.GetFullPath(storagePath)}' or files directory '{Path.GetFullPath(storageFilesPath)}'");
            }

            storageFileService.InitializeStorage();
        }
    }
}
