using FileStorage;
using FileStorage.Enums;
using FileStorage.Models;
using FileStorage.Services;
using FileStorage.ViewModels;

public class Controller
{
    private ConsolePrinter consolePrinter;
    private UserService userService;
    private StorageService storageService;

    public Controller(ConsolePrinter _consolePrinter)
    {
        consolePrinter = _consolePrinter;
        userService = new UserService();
        storageService = new StorageService();
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
}
