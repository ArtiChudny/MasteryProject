using FileStorage;
using FileStorage.Enums;
using FileStorage.Models;
using FileStorage.Services;
using FileStorage.ViewModels;
using System;
using System.Collections.Generic;

public class Controller
{
    private ConsolePrinter consolePrinter;
    private UserService userService;
    private StorageService storageService;
    private FileService fileService;

    public Controller(ConsolePrinter consolePrinter, StorageService storageService, FileService fileService)
    {
        this.consolePrinter = consolePrinter;
        this.storageService = storageService;
        this.fileService = fileService;
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
        }
    }

    private void ExecuteCommandGetUserInfo()
    {
        User user = userService.GetUser();
        StorageInfo storageInfo = storageService.GetStorageInfo();

        UserInfoViewModel userInfo = new UserInfoViewModel
        {
            Login = user.Login,
            UsedStorage = storageInfo.UsedStorage,
            CreationDate = storageInfo.CreationDate
        };

        consolePrinter.PrintUserInformation(userInfo);
    }

    private void ExecuteCommandFileUpload(List<string> parameters)
    {
        if (parameters.Count == 0)
        {
            throw new ApplicationException("You have to enter path to uploading file");
        }

        string filePath = parameters[0];
        StorageFile storageFile = fileService.UploadFileIntoStorage(filePath);
        storageService.AddFileToStorage(storageFile);

        FileUploadViewModel uploadViewModel = new FileUploadViewModel
        {
            FilePath = filePath,
            FileName = storageFile.FileName,
            FileSize = storageFile.Size.ToString(),
            Extension = storageFile.Extension
        };

        consolePrinter.PrintUploadSuccessful(uploadViewModel);
    }

    private void ExecuteCommandFileDownload(List<string> parameters)
    {
        if (parameters.Count == 0)
        {
            throw new ApplicationException("You have not entered parameters for this command");
        }

        string fileName = parameters[0];
        string destinationPath = parameters[1];

        fileService.DownloadFileFromStorage(fileName, destinationPath);
        storageService.IncreaseDownloadsCounter(fileName);

        consolePrinter.PrintDownloadSuccessful(fileName);
    }

    private void ExecuteCommandFileMove(List<string> parameters)
    {
        if (parameters.Count == 0)
        {
            throw new ApplicationException("You have not entered parameters for this command");
        }

        string oldFileName = parameters[0];
        string newFileName = parameters[1];

        fileService.MoveFile(oldFileName, newFileName);
        storageService.MoveFile(oldFileName, newFileName);

        consolePrinter.PrintMoveFileSuccessful(oldFileName, newFileName);
    }

    private void ExecuteCommandFileRemove(List<string> parameters)
    {
        if (parameters.Count == 0)
        {
            throw new ApplicationException("You have not entered parameters for this command");
        }

        string fileName = parameters[0];

        fileService.RemoveFile(fileName);
        storageService.RemoveFile(fileName);

        consolePrinter.PrintRemoveSuccessful(fileName);
    }
}
