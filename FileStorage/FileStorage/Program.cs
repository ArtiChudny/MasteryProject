using System;
using System.Collections.Generic;
using FileStorage.BLL.Interfaces;
using FileStorage.ConsoleUI.ConsoleUtils;
using FileStorage.ConsoleUI.ConsoleUtils.Interfaces;
using FileStorage.ConsoleUI.Enums;
using FileStorage.ConsoleUI.Models;
using FileStorage.ConsoleUI.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FileStorage.ConsoleUI.Helpers;

namespace FileStorage.ConsoleUI
{
    public class Program
    {
        private static readonly IServiceProvider Container = new DependencyContainer().GetContainer();

        static void Main(string[] args)
        {
            ILogger<Program> logger = Container.GetService<ILogger<Program>>();
            IConsolePrinter consolePrinter = Container.GetService<IConsolePrinter>();
            IAuthService authService = Container.GetService<IAuthService>();
            IStorageFileService storageFileService = Container.GetService<IStorageFileService>();
            Controller controller = Container.GetService<Controller>();
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
                        string logMessage = ConvertingHelper.GetLogMessage(ex.Message, ex.StackTrace);
                        logger.LogError(logMessage);
                        consolePrinter.PrintErrorMessage(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                string logMessage = ConvertingHelper.GetLogMessage(ex.Message, ex.StackTrace);
                logger.LogError(logMessage);
                consolePrinter.PrintErrorMessage(logMessage);
                Environment.Exit(-1);
            }
        }

        private static Credentials GetCredentials(Dictionary<StorageFlags, string> flags)
        {
            //TODO: NO NEED TO USE ELSE CONDITION HERE, YOU CAN JUST THROW EXCEPTION AFTER IF

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
            //TODO: USE TERNARY OPERATOR HERE.

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

            //TODO: POTENTIAL NullReferenceException. Ypu have to check response from client before you execute trim function
            string rowCommand = Console.ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(rowCommand))
            {
                throw new ApplicationException("You have not entered a command.");
            }

            return consoleCommandParser.Parse(rowCommand);
        }
    }
}
