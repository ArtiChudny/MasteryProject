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
    private ViewModelConverter viewModelConverter;

    public Controller(ConsolePrinter consolePrinter, StorageService storageService, FileService fileService)
    {
        this.consolePrinter = consolePrinter;
        this.storageService = storageService;
        this.fileService = fileService;
        userService = new UserService();
        viewModelConverter = new ViewModelConverter();
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
        StorageInfo storageInfo = storageService.GetStorageInfo();

        UserInfoViewModel userInfo = viewModelConverter.ConvertToUserInfoViewModel(user, storageInfo);
        consolePrinter.PrintUserInformation(userInfo);
    }

    private void ExecuteCommandFileUpload(List<string> parameters)
    {
        if (parameters.Count < 1)
        {
            throw new ApplicationException("You have to enter path to uploading the file");
        }

        string filePath = parameters[0];
        StorageFile storageFile = fileService.GetStorageFile(filePath);

        if (!storageService.IsFileSizeLessThanMaxSize(storageFile.Size))
        {
            throw new ApplicationException("The file exceeds the maximum size");
        }

        if (!storageService.IsEnoughStorageSpace(storageFile.Size))
        {
            throw new ApplicationException("Not enough space in storage to upload the file");
        }

        fileService.UploadFileIntoStorage(filePath);
        storageService.AddFileToStorage(storageFile);

        FileUploadViewModel uploadViewModel = viewModelConverter.ConvertToFileUploadViewModel(filePath, storageFile);
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

        fileService.DownloadFileFromStorage(fileName, destinationPath);
        storageService.IncreaseDownloadsCounter(fileName);

        if (fileService.IsMd5HashMatch(fileName, storageService.GetStorageFileMD5Hash(fileName)))
        {
            consolePrinter.PrintFileDownloadedSuccessful(fileName);
        }
        else
        {
            consolePrinter.PrintFileHasChanged(fileName);
        }
    }

    private void ExecuteCommandFileMove(List<string> parameters)
    {
        if (parameters.Count < 2)
        {
            throw new ApplicationException("You have not entered parameters for this command");
        }

        string oldFileName = parameters[0];
        string newFileName = parameters[1];

        fileService.MoveFile(oldFileName, newFileName);
        storageService.MoveFile(oldFileName, newFileName);

        consolePrinter.PrintFileMovedSuccessful(oldFileName, newFileName);
    }

    private void ExecuteCommandFileRemove(List<string> parameters)
    {
        if (parameters.Count < 1)
        {
            throw new ApplicationException("You have not entered the file name");
        }

        string fileName = parameters[0];
        fileService.RemoveFile(fileName);
        storageService.RemoveFile(fileName);

        consolePrinter.PrintFileRemovedSuccessful(fileName);
    }

    private void ExecuteCommandFileInfo(List<string> parameters)
    {
        if (parameters.Count < 1)
        {
            throw new ApplicationException("You have not entered the file name");
        }

        string fileName = parameters[0];
        StorageFile storageFile = storageService.GetFileInfo(fileName);
        User user = userService.GetUser();

        FileInfoViewModel fileInfoViewModel = viewModelConverter.ConvertToFileInfoViewModel(user, storageFile);
        consolePrinter.PrintFileInfo(fileInfoViewModel);
    }
}
