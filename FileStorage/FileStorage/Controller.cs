using FileStorage;
using FileStorage.Enums;
using FileStorage.Models;
using FileStorage.Services;
using FileStorage.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;

public class Controller
{
    private ConsolePrinter consolePrinter;
    private UserService userService;
    private StorageService storageService;
    private FileManager fileManager;

    public Controller(ConsolePrinter consolePrinter)
    {
        this.consolePrinter = consolePrinter;
        userService = new UserService();
        storageService = new StorageService();
        fileManager = new FileManager();
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
        string storagePath = ConfigurationManager.AppSettings["StoragePath"];
        if (parameters.Count == 0)
        {
            throw new ApplicationException("NO PATH BLEAT");
        }
        string filePath = parameters[0];
        fileManager.MoveFileToDestinationPath(filePath, storagePath);
    }
}
