using FileStorage.ConsoleUI.Enums;
using FileStorage.ConsoleUI.Models;
using FileStorage.ConsoleUI.ViewModels;
using FileStorage.ConsoleUI.Helpers;
using System;
using System.IO;
using FileStorage.DAL.Models;
using FileStorage.BLL.Interfaces;
using FileStorage.DAL.Constants;
using FileStorage.ConsoleUI.ConsoleUtils.Interfaces;
using Microsoft.Extensions.Logging;

public class Controller
{
    private IUserService userService;
    private IConsolePrinter consolePrinter;
    private IStorageFileService storageFileService;
    private ILogger<Controller> logger;

    public Controller(IStorageFileService storageFileService, IUserService userService, IConsolePrinter consolePrinter, ILogger<Controller> logger)
    {
        this.storageFileService = storageFileService;
        this.userService = userService;
        this.consolePrinter = consolePrinter;
        this.logger = logger;
    }

    public void ExecuteConsoleCommand(StorageCommand command)
    {
        switch (command.CommandType)
        {
            case StorageCommands.UserInfo:
                {
                    ExecuteCommandGetUserInfo(command.Options);
                    break;
                }
            case StorageCommands.FileUpload:
                {
                    ExecuteCommandFileUpload(command.Options);
                    break;
                }
            case StorageCommands.FileDownload:
                {
                    ExecuteCommandFileDownload(command.Options);
                    break;
                }
            case StorageCommands.FileMove:
                {
                    ExecuteCommandFileMove(command.Options);
                    break;
                }
            case StorageCommands.FileRemove:
                {
                    ExecuteCommandFileRemove(command.Options);
                    break;
                }
            case StorageCommands.FileInfo:
                {
                    ExecuteCommandFileInfo(command.Options);
                    break;
                }
            case StorageCommands.FileExport:
                {
                    ExecuteCommandFileExport(command.Options);
                    break;
                }
        }
    }

    private void ExecuteCommandGetUserInfo(Options options)
    {
        if (IsContainsRequiredNumberParameters(0, 0, options.Parameters.Count))
        {
            User user = userService.GetUser();
            StorageInfo storageInfo = storageFileService.GetStorageInfo();

            UserInfoViewModel userInfo = new UserInfoViewModel
            {
                Login = user.Login,
                UsedStorage = ConvertingHelper.GetSizeString(storageInfo.UsedStorage),
                CreationDate = ConvertingHelper.GetDateString(storageInfo.CreationDate)
            };

            consolePrinter.PrintUserInformation(userInfo);
        }
    }

    private void ExecuteCommandFileUpload(Options options)
    {
        if (IsContainsRequiredNumberParameters(1, 1, options.Parameters.Count))
        {
            string filePath = options.Parameters[0];
            StorageFile storageFile = storageFileService.UploadStorageFile(filePath);

            FileUploadViewModel uploadViewModel = new FileUploadViewModel
            {
                FilePath = filePath,
                FileName = Path.GetFileName(filePath),
                FileSize = ConvertingHelper.GetSizeString(storageFile.Size),
                Extension = storageFile.Extension
            };

            LogInformationMessage($"File \"{filePath}\" has been uploaded");
            consolePrinter.PrintFileUploadedSuccessful(uploadViewModel);
        }
    }

    private void ExecuteCommandFileDownload(Options options)
    {
        if (IsContainsRequiredNumberParameters(2, 2, options.Parameters.Count))
        {
            string fileName = options.Parameters[0];
            string destinationPath = options.Parameters[1];
            storageFileService.DownloadStorageFile(fileName, destinationPath);

            LogInformationMessage($"File \"{fileName}\" has been downloaded to {destinationPath}");
            consolePrinter.PrintFileDownloadedSuccessful(fileName);
        }
    }

    private void ExecuteCommandFileMove(Options options)
    {
        if (IsContainsRequiredNumberParameters(2, 2, options.Parameters.Count))
        {
            string oldFileName = options.Parameters[0];
            string newFileName = options.Parameters[1];
            storageFileService.MoveStorageFile(oldFileName, newFileName);

            LogInformationMessage($"File \"{oldFileName}\" has been moved to {newFileName}");
            consolePrinter.PrintFileMovedSuccessful(oldFileName, newFileName);
        }
    }

    private void ExecuteCommandFileRemove(Options options)
    {
        if (IsContainsRequiredNumberParameters(1, 1, options.Parameters.Count))
        {
            string fileName = options.Parameters[0];
            storageFileService.RemoveStorageFile(fileName);

            LogInformationMessage($"File \"{fileName}\" has been removed");
            consolePrinter.PrintFileRemovedSuccessful(fileName);
        }
    }

    private void ExecuteCommandFileInfo(Options options)
    {
        if (IsContainsRequiredNumberParameters(1, 1, options.Parameters.Count))
        {
            string fileName = options.Parameters[0];
            StorageFile storageFile = storageFileService.GetFileInfo(fileName);
            User user = userService.GetUser();

            FileInfoViewModel fileInfoViewModel = new FileInfoViewModel
            {
                FileName = fileName,
                Extension = storageFile.Extension,
                CreationDate = ConvertingHelper.GetDateString(storageFile.CreationDate),
                FileSize = ConvertingHelper.GetSizeString(storageFile.Size),
                DownloadsNumber = storageFile.DownloadsNumber,
                Login = user.Login
            };

            consolePrinter.PrintFileInfo(fileInfoViewModel);
        }
    }

    private void ExecuteCommandFileExport(Options options)
    {
        if (IsContainsRequiredNumberParameters(0, 1, options.Parameters.Count))
        {
            string[] formats = { FileFormats.Json, FileFormats.Xml };

            if (options.Parameters.Count == 0)
            {
                if (options.Flags.ContainsKey(StorageFlags.Info))
                {
                    consolePrinter.PrintExportFormats(formats);
                }
                else
                {
                    throw new ApplicationException("You have not entered parameters or flags for this command");
                }
            }

            if (options.Parameters.Count == 1)
            {
                string destinationPath = options.Parameters[0];
                string format = String.Empty;

                if (options.Flags.ContainsKey(StorageFlags.Format))
                {
                    format = options.Flags[StorageFlags.Format];
                }

                storageFileService.ExportFile(destinationPath, format);

                LogInformationMessage($"Meta-info file has been exported to \"{destinationPath}\"");
                consolePrinter.PrintExportSuccessfull(destinationPath);
            }
        }
    }

    private bool IsContainsRequiredNumberParameters(int minCount, int maxCount, int parametersCount)
    {
        if (parametersCount > maxCount)
        {
            throw new ApplicationException("Too much parameters for this command");
        }

        if (parametersCount < minCount)
        {
            throw new ApplicationException("You have not entered required parameters for this command");
        }

        return true;
    }

    private void LogInformationMessage(string message)
    {
        string logMessage = ConvertingHelper.GetLogMessage(message, string.Empty);
        logger.LogInformation(logMessage);
    }
}
