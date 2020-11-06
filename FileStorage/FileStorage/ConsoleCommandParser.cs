using System;
using System.Linq;
using FileStorage.Enums;
using FileStorage.Models;

namespace FileStorage
{
    public static class ConsoleCommandParser
    {
        public static StorageCommand Parse(string consoleInput)
        {
            StorageCommand command = new StorageCommand(); 
            string[] arguments = consoleInput.Split(" ");
            if (StorageCommands.commandsArray.Contains(arguments[0]))
            {
                command.CommandName = arguments[0];
                for (int argIndex = 1; argIndex < arguments.Length; argIndex++)
                {
                    command.Parameters.Add(arguments[argIndex]);
                }
                return command;
            }
            else
            {
                throw new ApplicationException($"Wrong command: {arguments[0]}.");
            }
        }
    }
}
