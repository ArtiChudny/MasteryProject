using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FileStorage.BLL.Enums;
using FileStorage.BLL.Models;

namespace FileStorage.ConsoleUI.ConsoleUtils
{
    public class ConsoleCommandParser
    {
        private readonly ConsoleFlagParser _consoleFlagParser;
        private const string UserInfoCommandName = "user info";
        private const string ExitCommandName = "exit";
        private const string FileUploadCommandName = "file upload";
        private const string FileDownloadCommandName = "file download";
        private const string FileMoveCommandName = "file move";
        private const string FileRemoveCommandName = "file remove";
        private const string FileInfoCommandName = "file info";
        private const string FileExportCommandName = "file export";
        private const string DirectoryCreateCommandName = "directory create";
        private const string DirectoryMoveCommandName = "directory move";
        private const string DirectoryRemoveCommandName = "directory remove";
        private const string DirectoryListCommandName = "directory list";
        private const string DirectorySearchCommandName = "directory search";
        private const string DirectoryInfoCommandName = "directory info";
        private const string FlagIndicator = "--";

        public ConsoleCommandParser(ConsoleFlagParser consoleFlagParser)
        {
            this._consoleFlagParser = consoleFlagParser;
        }

        public StorageCommand Parse(string rawCommand)
        {
            StorageCommand storageCommand = new StorageCommand();

            if (rawCommand.StartsWith(UserInfoCommandName))
            {
                storageCommand.CommandType = StorageCommands.UserInfo;
                storageCommand.Options = GetOptions(rawCommand, UserInfoCommandName);
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
                storageCommand.Options = GetOptions(rawCommand, FileUploadCommandName);
                return storageCommand;
            }

            if (rawCommand.StartsWith(FileDownloadCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileDownload;
                storageCommand.Options = GetOptions(rawCommand, FileDownloadCommandName);
                return storageCommand;
            }

            if (rawCommand.StartsWith(FileMoveCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileMove;
                storageCommand.Options = GetOptions(rawCommand, FileMoveCommandName);
                return storageCommand;
            }

            if (rawCommand.StartsWith(FileRemoveCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileRemove;
                storageCommand.Options = GetOptions(rawCommand, FileRemoveCommandName);
                return storageCommand;
            }

            if (rawCommand.StartsWith(FileInfoCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileInfo;
                storageCommand.Options = GetOptions(rawCommand, FileInfoCommandName);
                return storageCommand;
            }

            if (rawCommand.StartsWith(FileExportCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileExport;
                storageCommand.Options = GetOptions(rawCommand, FileExportCommandName);
                return storageCommand;
            }

            if (rawCommand.StartsWith(DirectoryCreateCommandName))
            {
                storageCommand.CommandType = StorageCommands.DirectoryCreate;
                storageCommand.Options = GetOptions(rawCommand, DirectoryCreateCommandName);
                return storageCommand;
            }

            if (rawCommand.StartsWith(DirectoryMoveCommandName))
            {
                storageCommand.CommandType = StorageCommands.DirectoryMove;
                storageCommand.Options = GetOptions(rawCommand, DirectoryMoveCommandName);
                return storageCommand;
            }

            if (rawCommand.StartsWith(DirectoryRemoveCommandName))
            {
                storageCommand.CommandType = StorageCommands.DirectoryRemove;
                storageCommand.Options = GetOptions(rawCommand, DirectoryRemoveCommandName);
                return storageCommand;
            }

            if (rawCommand.StartsWith(DirectoryListCommandName))
            {
                storageCommand.CommandType = StorageCommands.DirectoryList;
                storageCommand.Options = GetOptions(rawCommand, DirectoryListCommandName);
                return storageCommand;
            }

            if (rawCommand.StartsWith(DirectorySearchCommandName))
            {
                storageCommand.CommandType = StorageCommands.DirectorySearch;
                storageCommand.Options = GetOptions(rawCommand, DirectorySearchCommandName);
                return storageCommand;
            }

            if (rawCommand.StartsWith(DirectoryInfoCommandName))
            {
                storageCommand.CommandType = StorageCommands.DirectoryInfo;
                storageCommand.Options = GetOptions(rawCommand, DirectoryInfoCommandName);
                return storageCommand;
            }

            throw new ApplicationException($"Wrong command: {rawCommand}.");
        }

        private Options GetOptions(string rawCommand, string commandName)
        {
            string parametersString = rawCommand.Replace(commandName, string.Empty).Trim();
            string regPattern = @"(""[\w\s\\\/:.-]*"")|(-?-?[\w.]*)";

            List<string> parametersList = Regex.Matches(parametersString, regPattern, RegexOptions.Multiline).Select(m => m.Value).ToList();
            parametersList.RemoveAll(item => item == string.Empty);

            var options = new Options();

            for (int listIndex = 0; listIndex < parametersList.Count; listIndex++)
            {
                if (parametersList[listIndex].StartsWith(FlagIndicator))
                {
                    string[] flags = parametersList.GetRange(listIndex, parametersList.Count - listIndex).ToArray();
                    options.Flags = _consoleFlagParser.Parse(flags);
                    break;
                }
                else
                {
                    string parameter = parametersList[listIndex].Replace("\"", string.Empty).Trim();
                    options.Parameters.Add(parameter);
                }
            }

            return options;
        }
    }
}
