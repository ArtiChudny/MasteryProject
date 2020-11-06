using System;
using System.Collections.Generic;
using FileStorage.Enums;
using FileStorage.Models;

namespace FileStorage
{
    public static class ConsoleCommandParser
    {
        private const string UserInfo = "user info";
        private const string Exit = "exit";

        public static StorageCommand Parse(string rawCommand)
        {
            StorageCommand command = new StorageCommand();
            string[] arguments = rawCommand.Split(" ");
            command.CommandType = GetCommandType(arguments[0]);
            if (arguments.Length > 1)
            {
                command.Parameters = GetParametersList(arguments);
            }
            return command;
        }

        private static StorageCommands GetCommandType(string commandName)
        {
            switch (commandName)
            {
                case UserInfo:
                    return StorageCommands.UserInfo;
                case Exit:
                    return StorageCommands.Exit;
                default:
                    throw new ApplicationException($"Wrong command: {commandName}.");
            }
        }

        private static List<string> GetParametersList(string[] arguments)
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

