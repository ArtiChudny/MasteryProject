﻿using FileStorage;
using FileStorage.Enums;
using FileStorage.Models;
using FileStorage.Services;
using FileStorage.ViewModels;
using FileStorage.Helpers;
using System;
using System.IO;

public class Controller
{
    private ConsolePrinter consolePrinter;
    private UserService userService;
    private StorageService storageService;
    private FileService fileService;
    private StorageFileService storageFileService;

    public Controller(ConsolePrinter consolePrinter, StorageService storageService, FileService fileService)
    {
        this.consolePrinter = consolePrinter;
        this.storageService = storageService;
        this.fileService = fileService;
        storageFileService = new StorageFileService(storageService, fileService);
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

    private void ExecuteCommandFileUpload(Options options)
    {
        if (options.Parameters.Count < 1)
        {
            throw new ApplicationException("You have to enter path to uploading the file");
        }

        string filePath = options.Parameters[0];
        StorageFile storageFile = storageFileService.UploadStorageFile(filePath);

        FileUploadViewModel uploadViewModel = new FileUploadViewModel
        {
            FilePath = filePath,
            FileName = Path.GetFileName(filePath),
            FileSize = ConvertingHelper.GetSizeString(storageFile.Size),
            Extension = storageFile.Extension
        };

        consolePrinter.PrintFileUploadedSuccessful(uploadViewModel);
    }

    private void ExecuteCommandFileDownload(Options options)
    {
        if (options.Parameters.Count < 2)
        {
            throw new ApplicationException("You have not entered parameters for this command");
        }

        string fileName = options.Parameters[0];
        string destinationPath = options.Parameters[1];
        storageFileService.DownloadStorageFile(fileName, destinationPath);

        consolePrinter.PrintFileDownloadedSuccessful(fileName);
    }

    private void ExecuteCommandFileMove(Options options)
    {
        if (options.Parameters.Count < 2)
        {
            throw new ApplicationException("You have not entered parameters for this command");
        }

        string oldFileName = options.Parameters[0];
        string newFileName = options.Parameters[1];
        storageFileService.MoveStorageFile(oldFileName, newFileName);

        consolePrinter.PrintFileMovedSuccessful(oldFileName, newFileName);
    }

    private void ExecuteCommandFileRemove(Options options)
    {
        if (options.Parameters.Count < 1)
        {
            throw new ApplicationException("You have not entered the file name");
        }

        string fileName = options.Parameters[0];
        storageFileService.RemoveStorageFile(fileName);

        consolePrinter.PrintFileRemovedSuccessful(fileName);
    }

    private void ExecuteCommandFileInfo(Options options)
    {
        if (options.Parameters.Count < 1)
        {
            throw new ApplicationException("You have not entered the file name");
        }

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

    private void ExecuteCommandFileExport(Options options)
    {
        string[] formats = { "json", "xml" };

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

            consolePrinter.PrintExportSuccessfull(destinationPath);
        }
    }
}
