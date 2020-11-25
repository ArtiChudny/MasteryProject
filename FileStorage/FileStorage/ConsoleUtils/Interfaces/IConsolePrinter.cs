using FileStorage.ConsoleUI.ViewModels;

namespace FileStorage.ConsoleUI.ConsoleUtils.Interfaces
{
    public interface IConsolePrinter
    {
        void PrintUserInformation(UserInfoViewModel userInfo);
        void PrintFileUploadedSuccessful(FileUploadViewModel uploadViewModel);
        void PrintFileInfo(FileInfoViewModel fileInfoViewModel);
        void PrintFileDownloadedSuccessful(string fileName);
        void PrintFileMovedSuccessful(string oldFileName, string newFileName);
        void PrintFileRemovedSuccessful(string fileName);
        void PrintAuthenticationSuccessful();
        void PrintСommandWaitingIcon();
        void PrintErrorMessage(string errorMessage);
        void PrintExitMessage();
        void PrintExportFormats(string[] formats);
        void PrintExportSuccessfull(string destinationPath);
        void PrintCreateDirectorySuccessfull(string destinationPath, string directoryName);
        void PrintMoveDirectorySuccessfull(string oldPath, string newPath);
    }
}
