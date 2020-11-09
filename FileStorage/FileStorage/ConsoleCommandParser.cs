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

            if (rawCommand.StartsWith(Exit))
            {
                storageCommand.CommandType = StorageCommands.Exit;
                return storageCommand;
            }

            throw new ApplicationException($"Wrong command: {rawCommand}.");
        }

        private List<string> GetParametersList(string rawCommand, string commandName)
        {
            List<string> parametersList = new List<string>();
            string parametersString = rawCommand.Replace(commandName, string.Empty);

            if (parametersString != string.Empty)
            {
                string[] parameters = parametersString.Split(" ");
                for (int argIndex = 1; argIndex < parameters.Length; argIndex++)
                {
                    parametersList.Add(parameters[argIndex]);
                }
            }

            return parametersList;
        }
    }
}


