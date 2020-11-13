using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FileStorage.Enums;
using FileStorage.Models;

namespace FileStorage
{
    public class ConsoleCommandParser
    {
        private const string UserInfoCommandName = "user info";
        private const string ExitCommandName = "exit";
        private const string FileUploadCommandName = "file upload";
        private const string FileDownloadCommandName = "file download";
        private const string FileMoveCommandName = "file move";
        private const string FileRemoveCommandName = "file remove";
        private const string FileInfoCommandName = "file info";
        private const string FileExportCommandName = "file export";

        public StorageCommand Parse(string rawCommand)
        {
            StorageCommand storageCommand = new StorageCommand();
            string lowerRawCommand = rawCommand.ToLower();

            if (lowerRawCommand.StartsWith(UserInfoCommandName))
            {
                storageCommand.CommandType = StorageCommands.UserInfo;
                storageCommand.Parameters = GetParametersList(rawCommand, UserInfoCommandName);
                return storageCommand;
            }

            if (lowerRawCommand.StartsWith(ExitCommandName))
            {
                storageCommand.CommandType = StorageCommands.Exit;
                return storageCommand;
            }

            if (lowerRawCommand.StartsWith(FileUploadCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileUpload;
                storageCommand.Parameters = GetParametersList(rawCommand, FileUploadCommandName);
                return storageCommand;
            }

            if (lowerRawCommand.StartsWith(FileDownloadCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileDownload;
                storageCommand.Parameters = GetParametersList(rawCommand, FileDownloadCommandName);
                return storageCommand;
            }

            if (lowerRawCommand.StartsWith(FileMoveCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileMove;
                storageCommand.Parameters = GetParametersList(rawCommand, FileMoveCommandName);
                return storageCommand;
            }

            if (lowerRawCommand.StartsWith(FileRemoveCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileRemove;
                storageCommand.Parameters = GetParametersList(rawCommand, FileRemoveCommandName);
                return storageCommand;
            }

            if (lowerRawCommand.StartsWith(FileInfoCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileInfo;
                storageCommand.Parameters = GetParametersList(rawCommand, FileInfoCommandName);
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
                string[] parameters = parametersString.Split(" \"");
                for (int argIndex = 0; argIndex < parameters.Length; argIndex++)
                {
                    parametersList.Add(parameters[argIndex].Trim().Replace("\"", string.Empty));
                }
            }

            return parametersList;
        }
    }
}


