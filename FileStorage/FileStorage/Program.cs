using System;
using System.Collections.Generic;
using FileStorage.BLL;
using FileStorage.BLL.Interfaces;
using FileStorage.ConsoleUI.ConsoleUtils;
using FileStorage.ConsoleUI.ConsoleUtils.Interfaces;
using FileStorage.DAL.Repositories;
using FileStorage.DAL.Repositories.Interfaces;
using FileStorage.ConsoleUI.Enums;
using FileStorage.ConsoleUI.Models;

namespace FileStorage.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IUserRepository userRepository = new UserRepository();
            IFileRepository fileRepository = new FileRepository();
            IStorageRepository storageRepository = new StorageRepository();
            IConsolePrinter consolePrinter = new ConsolePrinter();

            IAuthService authService = new AuthService(userRepository);
            IUserService userService = new UserService(userRepository);
            IStorageFileService storageFileService = new StorageFileService(storageRepository, fileRepository);

            Controller controller = new Controller(storageFileService, userService, consolePrinter);
            ConsoleFlagParser consoleFlagParser = new ConsoleFlagParser();
            ConsoleCommandParser consoleCommandParser = new ConsoleCommandParser(consoleFlagParser);

            try
            {
                storageFileService.InitializeStorage();
                Dictionary<StorageFlags, string> flags = consoleFlagParser.Parse(args);
                Credentials credentials = GetCredentials(flags);

                if (!authService.IsAuthenticated(credentials.Login, credentials.Password))
                {
                    throw new ApplicationException("Incorrect login or password");
                }
                
                consolePrinter.PrintAuthenticationSuccessful();
                
                StorageCommand command = new StorageCommand();
                while (command.CommandType != StorageCommands.Exit)
                {
                    try
                    {
                        command = GetCommand(consoleCommandParser, consolePrinter);
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

        private static StorageCommand GetCommand(ConsoleCommandParser consoleCommandParser, IConsolePrinter consolePrinter)
        {
            consolePrinter.PrintСommandWaitingIcon();
            string rowCommand = Console.ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(rowCommand))
            {
                throw new ApplicationException("You have not entered a command.");
            }

            return consoleCommandParser.Parse(rowCommand);
        }
    }
}
