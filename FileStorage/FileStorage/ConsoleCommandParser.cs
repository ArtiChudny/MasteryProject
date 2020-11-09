using System;
using System.Collections.Generic;
using FileStorage.Enums;
using FileStorage.Models;

namespace FileStorage
{
    public class ConsoleCommandParser
    {
        private const string UserInfoCommandName = "user info";
        private const string ExitCommandName = "exit";
        private const string FileUploadCommandName = "file upload";

        public StorageCommand Parse(string rawCommand)
        {
            StorageCommand storageCommand = new StorageCommand();

            if (rawCommand.StartsWith(UserInfoCommandName))
            {
                storageCommand.CommandType = StorageCommands.UserInfo;
                storageCommand.Parameters = GetParametersList(rawCommand, UserInfoCommandName);
                return storageCommand;
            }

            if (rawCommand.StartsWith(ExitCommandName))
            {
                storageCommand.CommandType = StorageCommands.Exit;
                return storageCommand;
            }

            if (rawCommand.StartsWith(FileUploadCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileUpload;
                storageCommand.Parameters = GetParametersList(rawCommand, FileUploadCommandName);
                return storageCommand;
            }

            throw new ApplicationException($"Wrong command: {rawCommand}.");
        }

        private List<string> GetParametersList(string rawCommand, string commandName)
        {
            List<string> parametersList = new List<string>();
            string parametersString = rawCommand.Replace(commandName, string.Empty).Trim();

            if (parametersString != string.Empty)
            {
                string[] parameters = parametersString.Split(" ");
                for (int argIndex = 0; argIndex < parameters.Length; argIndex++)
                {
                    parametersList.Add(parameters[argIndex]);
                }
            }

            return parametersList;
        }
    }
}


