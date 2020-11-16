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
        private ConsoleFlagParser consoleFlagParser;
        private const string UserInfoCommandName = "user info";
        private const string ExitCommandName = "exit";
        private const string FileUploadCommandName = "file upload";
        private const string FileDownloadCommandName = "file download";
        private const string FileMoveCommandName = "file move";
        private const string FileRemoveCommandName = "file remove";
        private const string FileInfoCommandName = "file info";
        private const string FileExportCommandName = "file export";
        private const string FlagIndicator = "--";

        public ConsoleCommandParser(ConsoleFlagParser consoleFlagParser)
        {
            this.consoleFlagParser = consoleFlagParser;
        }

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

            if (lowerRawCommand.StartsWith(FileExportCommandName))
            {
                storageCommand.CommandType = StorageCommands.FileExport;
                storageCommand.Options = GetOptions(rawCommand, FileExportCommandName);
                return storageCommand;
            }

            throw new ApplicationException($"Wrong command: {rawCommand}.");
        }

        private Options GetOptions(string rawCommand, string commandName)
        {
            string parametersString = rawCommand.Replace(commandName, string.Empty).Trim();
            string regPattern = @"(""[\w\s\\\/:.]*"")|(-?-?[\w.]*)";

            List<string> parametersList = Regex.Matches(parametersString, regPattern, RegexOptions.Multiline).Cast<Match>().Select(m => m.Value).ToList();
            parametersList.RemoveAll(item => item == string.Empty);

            Options options = new Options();

            for (int listIndex = 0; listIndex < parametersList.Count; listIndex++)
            {
                if (parametersList[listIndex].StartsWith(FlagIndicator))
                {
                    string[] flags = parametersList.GetRange(listIndex, parametersList.Count - listIndex).ToArray();
                    options.Flags = consoleFlagParser.Parse(flags);
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
