using FileStorage;
using FileStorage.Enums;
using FileStorage.Models;
using FileStorage.Services;
using FileStorage.ViewModels;

public class Controller
{
    ConsolePrinter consolePrinter;
    public Controller(ConsolePrinter _consolePrinter)
    {
        consolePrinter = _consolePrinter;
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
            case StorageCommands.Exit:
                {
                    ExitApplication();
                    break;
                }
        }
    }

    private void ExecuteCommandGetUserInfo()
    {
        User user = UserService.GetUser();
        StorageInfo storageInfo = StorageService.GetStorageInfo();
        UserInfoViewModel userInfo = new UserInfoViewModel
        {
            Login = user.Login,
            UsedStorage = storageInfo.UsedStorage,
            CreationDate = storageInfo.CreationDate
        };

        consolePrinter.PrintUserInformation(userInfo);
    }

    public void ExitApplication()
    {
        consolePrinter.PrintExitMessage();
    }

}
