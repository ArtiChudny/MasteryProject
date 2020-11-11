using FileStorage;
using FileStorage.Enums;
using FileStorage.Models;
using FileStorage.Services;
using FileStorage.ViewModels;
using FileStorage.Helpers;
using System;
using System.Collections.Generic;

public class Controller
{
    private ConsolePrinter consolePrinter;
    private UserService userService;
    private StorageFileService storageFileService;

    public Controller(ConsolePrinter consolePrinter, StorageFileService storageFileService)
    {
        this.consolePrinter = consolePrinter;
        this.storageFileService = storageFileService;
        userService = new UserService();
    }

    public void ExecuteConsoleCommand(StorageCommand command)
    {
        switch (command.CommandType)
        {
            case StorageCommands.UserInfo:
                {
                    ExecuteCommandGetUserInfo();
                    break;
                }
            case StorageCommands.FileUpload:
                {
                    ExecuteCommandFileUpload(command.Parameters);
                    break;
                }
            case StorageCommands.FileDownload:
                {
                    ExecuteCommandFileDownload(command.Parameters);
                    break;
                }
            case StorageCommands.FileMove:
                {
                    ExecuteCommandFileMove(command.Parameters);
                    break;
                }
            case StorageCommands.FileRemove:
                {
                    ExecuteCommandFileRemove(command.Parameters);
                    break;
                }
            case StorageCommands.FileInfo:
                {
                    ExecuteCommandFileInfo(command.Parameters);
                    break;
                }
        }
    }

    private void ExecuteCommandGetUserInfo()
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

    private void ExecuteCommandFileUpload(List<string> parameters)
    {
        if (parameters.Count < 1)
        {
            throw new ApplicationException("You have to enter path to uploading the file");
        }

        string filePath = parameters[0];
        StorageFile storageFile = storageFileService.UploadStorageFile(filePath);

        FileUploadViewModel uploadViewModel = new FileUploadViewModel
        {
            FilePath = filePath,
            FileName = storageFile.FileName,
            FileSize = ConvertingHelper.GetSizeString(storageFile.Size),
            Extension = storageFile.Extension
        };

        consolePrinter.PrintFileUploadedSuccessful(uploadViewModel);
    }

    private void ExecuteCommandFileDownload(List<string> parameters)
    {
        if (parameters.Count < 2)
        {
            throw new ApplicationException("You have not entered parameters for this command");
        }

        string fileName = parameters[0];
        string destinationPath = parameters[1];
        storageFileService.DownloadStorageFile(fileName, destinationPath);

        consolePrinter.PrintFileDownloadedSuccessful(fileName);
    }

    private void ExecuteCommandFileMove(List<string> parameters)
    {
        if (parameters.Count < 2)
        {
            throw new ApplicationException("You have not entered parameters for this command");
        }

        string oldFileName = parameters[0];
        string newFileName = parameters[1];
        storageFileService.MoveStorageFile(oldFileName, newFileName);

        consolePrinter.PrintFileMovedSuccessful(oldFileName, newFileName);
    }

    private void ExecuteCommandFileRemove(List<string> parameters)
    {
        if (parameters.Count < 1)
        {
            throw new ApplicationException("You have not entered the file name");
        }

        string fileName = parameters[0];
        storageFileService.RemoveStorageFile(fileName);

        consolePrinter.PrintFileRemovedSuccessful(fileName);
    }

    private void ExecuteCommandFileInfo(List<string> parameters)
    {
        if (parameters.Count < 1)
        {
            throw new ApplicationException("You have not entered the file name");
        }

        string fileName = parameters[0];
        StorageFile storageFile = storageFileService.GetFileInfo(fileName);
        User user = userService.GetUser();

        FileInfoViewModel fileInfoViewModel = new FileInfoViewModel
        {
            FileName = storageFile.FileName,
            Extension = storageFile.Extension,
            CreationDate = ConvertingHelper.GetDateString(storageFile.CreationDate),
            FileSize = ConvertingHelper.GetSizeString(storageFile.Size),
            DownloadsNumber = storageFile.DownloadsNumber,
            Login = user.Login
        };

        consolePrinter.PrintFileInfo(fileInfoViewModel);
    }
}
