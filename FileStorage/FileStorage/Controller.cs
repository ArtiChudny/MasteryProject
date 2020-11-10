﻿using FileStorage;
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
    private Converter converter;

    public Controller(ConsolePrinter consolePrinter, StorageService storageService, FileService fileService)
    {
        this.consolePrinter = consolePrinter;
        this.storageService = storageService;
        this.fileService = fileService;
        userService = new UserService();
        converter = new Converter();
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

        UserInfoViewModel userInfo = new UserInfoViewModel
        {
            Login = user.Login,
            UsedStorage = converter.GetSizeString(storageInfo.UsedStorage),
            CreationDate = converter.GetDateString(storageInfo.CreationDate)
        };

        consolePrinter.PrintUserInformation(userInfo);
    }

    private void ExecuteCommandFileUpload(List<string> parameters)
    {
        if (parameters.Count == 0)
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

        FileUploadViewModel uploadViewModel = new FileUploadViewModel
        {
            FilePath = filePath,
            FileName = storageFile.FileName,
            FileSize = converter.GetSizeString(storageFile.Size),
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

    private void ExecuteCommandFileInfo(List<string> parameters)
    {
        if (parameters.Count == 0)
        {
            throw new ApplicationException("You have not entered parameters for this command");
        }

        string fileName = parameters[0];
        StorageFile storageFile = storageService.GetFileInfo(fileName);
        User user = userService.GetUser();

        FileInfoViewModel fileInfoViewModel = new FileInfoViewModel
        {
            FileName = storageFile.FileName,
            Extension = storageFile.Extension,
            CreationDate = converter.GetDateString(storageFile.CreationDate),
            FileSize = converter.GetSizeString(storageFile.Size),
            DownloadsNumber = storageFile.DownloadsNumber,
            Login = user.Login
        };

        consolePrinter.PrintFileInfo(fileInfoViewModel);
    }
}
