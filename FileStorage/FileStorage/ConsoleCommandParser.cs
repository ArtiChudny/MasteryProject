using System;
using System.Collections.Generic;
using FileStorage.Enums;
using FileStorage.Models;

namespace FileStorage
{
    public class ConsoleCommandParser
    {
        private const string UserInfo = "user info";
        private const string Exit = "exit";

        public StorageCommand Parse(string rawCommand)
        {
            StorageCommand command = new StorageCommand();
            command = GetCommand(rawCommand);

            return command;
        }

        private StorageCommand GetCommand(string rawCommand)
        {
            StorageCommand storageCommand = new StorageCommand();
            if (rawCommand.StartsWith(UserInfo))
            {
                storageCommand.CommandType = StorageCommands.UserInfo;
                storageCommand.Parameters = GetParametersList(rawCommand, UserInfo);
                return storageCommand;
            }
            else if (rawCommand.StartsWith(Exit))
            {
                storageCommand.CommandType = StorageCommands.Exit;
                return storageCommand;
            }
            else
            {
                throw new ApplicationException($"Wrong command: {rawCommand}.");
            }
        }

        private List<string> GetParametersList(string rawCommand, string commandType)
        {
            List<string> parametersList = new List<string>();
            string parametersString = rawCommand.Replace(commandType, string.Empty);
            string[] parameters = parametersString.Split(" ");
            if (parameters.Length > 1)
            {
                for (int argIndex = 1; argIndex < parameters.Length; argIndex++)
                {
                    parametersList.Add(parameters[argIndex]);
                }
            }

            return parametersList;
        }

        private List<string> GetParametersList(string[] arguments)
        {
            List<string> parametersList = new List<string>();
            for (int argIndex = 1; argIndex < arguments.Length; argIndex++)
            {
                parametersList.Add(arguments[argIndex]);
            }

            return parametersList;
        }
    }
}

