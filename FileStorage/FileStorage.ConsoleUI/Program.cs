using System;
using System.Collections.Generic;
using FileStorage.ConsoleUI.ConsoleUtils;
using FileStorage.ConsoleUI.ConsoleUtils.Interfaces;
using FileStorage.ConsoleUI.Models;
using FileStorage.ConsoleUI.IoC;
using FileStorage.ConsoleUI.Controllers;
using FileStorage.ConsoleUI.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FileStorage.BLL.Enums;
using FileStorage.BLL.Commands;
using FileStorage.BLL.Queries;
using FileStorage.BLL.Models;
using MediatR;

namespace FileStorage.ConsoleUI
{
    public class Program
    {
        private static readonly IServiceProvider _container = new DependencyContainer().GetContainer();

        static void Main(string[] args)
        {
            var logger = _container.GetService<ILogger<Program>>();
            var consolePrinter = _container.GetService<IConsolePrinter>();
            var controller = _container.GetService<IController>();
            var mediator = _container.GetService<IMediator>();
            var consoleFlagParser = new ConsoleFlagParser();
            var consoleCommandParser = new ConsoleCommandParser(consoleFlagParser);

            try
            {
                Console.InputEncoding = System.Text.Encoding.Unicode;
                Console.OutputEncoding = System.Text.Encoding.Unicode;

                mediator.Send(new InitializeStorageCommand());

                var argsFlags = consoleFlagParser.Parse(args);
                var credentials = GetCredentials(argsFlags);
                var authQuery = new IsAuthenticatedQuery(credentials.Login, credentials.Password);

                if (!mediator.Send(authQuery).Result)
                {
                    throw new ArgumentException("Incorrect login or password");
                }

                consolePrinter.PrintAuthenticationSuccessful();
                var command = new StorageCommand();

                while (command.CommandType != StorageCommands.Exit)
                {
                    try
                    {
                        command = GetCommand(consoleCommandParser, consolePrinter);
                        var task = controller.ExecuteConsoleCommand(command);
                        task.Wait();
                    }
                    catch (AggregateException agEx)
                    {
                        foreach (var inEx in agEx.InnerExceptions)
                        {
                            string logMessage = ConvertingHelper.GetLogMessage(inEx.Message, inEx.StackTrace);
                            logger.LogError(logMessage);
                            consolePrinter.PrintErrorMessage(inEx.Message);
                        }
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

            throw new ArgumentException("You have to enter your login and password. Use --l for login and --p for password.");
        }

        private static bool IsContainLoginPassword(Dictionary<StorageFlags, string> flags)
        {
            return flags.ContainsKey(StorageFlags.Login) && flags.ContainsKey(StorageFlags.Password);
        }

        private static StorageCommand GetCommand(ConsoleCommandParser consoleCommandParser, IConsolePrinter consolePrinter)
        {
            consolePrinter.PrintСommandWaitingIcon();
            string rowCommand = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(rowCommand))
            {
                throw new ArgumentNullException("You have not entered a command.");
            }

            return consoleCommandParser.Parse(rowCommand.Trim());
        }
    }
}
