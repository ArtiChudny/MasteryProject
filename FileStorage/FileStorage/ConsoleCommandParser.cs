using System;
using System.Collections.Generic;
using System.Linq;
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
                storageCommand.Options = GetOptions(rawCommand, UserInfoCommandName);
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
                storageCommand.Options = GetOptions(rawCommand, FileUploadCommandName);
                return storageCommand;
            }

            if (lowerRawCommand.StartsWith(FileDownloadCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileDownload;
                storageCommand.Options = GetOptions(rawCommand, FileDownloadCommandName);
                return storageCommand;
            }

            if (lowerRawCommand.StartsWith(FileMoveCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileMove;
                storageCommand.Options = GetOptions(rawCommand, FileMoveCommandName);
                return storageCommand;
            }

            if (lowerRawCommand.StartsWith(FileRemoveCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileRemove;
                storageCommand.Options = GetOptions(rawCommand, FileRemoveCommandName);
                return storageCommand;
            }

            if (lowerRawCommand.StartsWith(FileInfoCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileInfo;
                storageCommand.Options = GetOptions(rawCommand, FileInfoCommandName);
                return storageCommand;
            }

            throw new ApplicationException($"Wrong command: {rawCommand}.");
        }

        private Options GetOptions(string rawCommand, string commandName)
        {
            string parametersString = rawCommand.Replace(commandName, string.Empty).Trim();
            string regPattern = @"""([\w\s\/.]*)""|(-?-?[\w.]*)";

            List<string> parametersList = Regex.Matches(parametersString, regPattern).Cast<Match>().Select(m => m.Value).ToList();
            parametersList.RemoveAll(item => item == string.Empty);

            Options options = new Options();

            for (int i = 0; i < parametersList.Count; i++)
            {
                if (parametersList[i].StartsWith("--"))
                {
                    StorageFlags flag = ConsoleFlagParser.GetFlag(parametersList[i]);
                    string flagValue = string.Empty;

                    if (i + 1 < parametersList.Count)
                    {
                        if (!parametersList[i + 1].StartsWith("--"))
                        {
                            flagValue = parametersList[i + 1];
                            i++;
                        }
                    }

                    options.Flags.Add(flag, flagValue);
                }
                else
                {
                    string parameter = parametersList[i].Replace("\"", string.Empty).Trim();
                    options.Parameters.Add(parameter);
                }
            }

            return options;
        }
    }
}
