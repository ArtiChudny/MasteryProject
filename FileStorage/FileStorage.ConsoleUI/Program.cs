using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
using System.Diagnostics;

namespace FileStorage.ConsoleUI
{
    public class Program
    {
        private static readonly IServiceProvider _container = new DependencyContainer().GetContainer();

        static async Task Main(string[] args)
        {
            var logger = _container.GetService<ILogger<Program>>();
            var consolePrinter = _container.GetService<IConsolePrinter>();
            var controller = _container.GetService<IController>();
            var mediator = _container.GetService<IMediator>();
            var consoleFlagParser = new ConsoleFlagParser();
            var consoleCommandParser = new ConsoleCommandParser(consoleFlagParser);

            try
            {
                await mediator.Send(new InitializeStorageCommand());

                var argsFlags = consoleFlagParser.Parse(args);
                var credentials = GetCredentials(argsFlags);             
                
                var isAuthQuery = new IsAuthenticatedQuery(credentials.Login, credentials.Password);
                await mediator.Send(isAuthQuery);
                consolePrinter.PrintAuthenticationSuccessful();

                var command = new StorageCommand();
                while (command.CommandType != StorageCommands.Exit)
                {
                    try
                    {
                        command = GetCommand(consoleCommandParser, consolePrinter);
                     
                        var stopWatch = new Stopwatch();
                        stopWatch.Start();
                        await controller.ExecuteConsoleCommand(command);
                        stopWatch.Stop();

                        var saveExecutionInfoCommand = new SaveExecutionInfoCommand(command.CommandType.ToString(), stopWatch.Elapsed.TotalMilliseconds);
                        await mediator.Send(saveExecutionInfoCommand);
                    }
                    catch (AggregateException agEx)
                    {
                        foreach (var innerEx in agEx.InnerExceptions)
                        {
                            string logMessage = ConvertingHelper.GetLogMessage(innerEx.Message, innerEx.StackTrace);
                            logger.LogError(logMessage);
                            consolePrinter.PrintErrorMessage(innerEx.Message);
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
                throw new ArgumentException("You have not entered a command.");
            }

            return consoleCommandParser.Parse(rowCommand.ToLower().Trim());
        }
    }
}
