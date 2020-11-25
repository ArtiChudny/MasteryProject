using System;
using FileStorage.ConsoleUI.ConsoleUtils.Interfaces;
using FileStorage.ConsoleUI.ViewModels;

namespace FileStorage.ConsoleUI.ConsoleUtils
{
    public class ConsolePrinter : IConsolePrinter
    {
        public void PrintUserInformation(UserInfoViewModel userInfo)
        {
            Console.WriteLine("\nlogin: {0}", userInfo.Login);
            Console.WriteLine("creation date: {0}", userInfo.CreationDate);
            Console.WriteLine("storage used: {0}\n", userInfo.UsedStorage);
        }

        public void PrintFileUploadedSuccessful(FileUploadViewModel uploadViewModel)
        {
            Console.WriteLine("\nThe file '{0}' has been uploaded.", uploadViewModel.FilePath);
            Console.WriteLine("- file name: '{0}'", uploadViewModel.FileName);
            Console.WriteLine("- file size: '{0}'", uploadViewModel.FileSize);
            Console.WriteLine("- extension: '{0}'\n", uploadViewModel.Extension);
        }

        public void PrintFileInfo(FileInfoViewModel fileInfoViewModel)
        {
            Console.WriteLine("\n- file name: '{0}'", fileInfoViewModel.FileName);
            Console.WriteLine("- extension: '{0}'", fileInfoViewModel.Extension);
            Console.WriteLine("- file size: '{0}'", fileInfoViewModel.FileSize);
            Console.WriteLine("- creation date: '{0}'", fileInfoViewModel.CreationDate);
            Console.WriteLine("- downloads number: '{0}'", fileInfoViewModel.DownloadsNumber);
            Console.WriteLine("- login: '{0}'\n", fileInfoViewModel.Login);
        }

        public void PrintFileDownloadedSuccessful(string fileName)
        {
            Console.WriteLine("\nThe file '{0}' has been downloaded\n", fileName);
        }

        public void PrintFileMovedSuccessful(string oldFileName, string newFileName)
        {
            Console.WriteLine("\nThe file '{0}' has been moved to '{1}'\n", oldFileName, newFileName);
        }

        public void PrintFileRemovedSuccessful(string fileName)
        {
            Console.WriteLine("\nThe file '{0}' has been removed\n", fileName);
        }

        public void PrintAuthenticationSuccessful()
        {
            Console.WriteLine("You logged in\n");
        }

        public void PrintСommandWaitingIcon()
        {
            Console.Write(">");
        }

        public void PrintErrorMessage(string errorMessage)
        {
            Console.WriteLine("\n{0}\n", errorMessage);
        }

        public void PrintExitMessage()
        {
            Console.WriteLine("You have been exit the application");
        }

        public void PrintExportFormats(string[] formats)
        {
            Console.WriteLine();
            foreach (var format in formats)
            {
                Console.WriteLine("- {0}", format);
            }
            Console.WriteLine();
        }

        public void PrintExportSuccessfull(string destinationPath)
        {
            Console.WriteLine("The meta-information has been exported, path = \"{0}\"", destinationPath);
        }

        public void PrintCreateDirectorySuccessfull(string destinationPath, string directoryName)
        {
            Console.WriteLine("\nDirectory {0} has been succesfully created at the path '{1}'\n", directoryName, destinationPath);
        }

        public void PrintMoveDirectorySuccessfull(string oldPath, string newPath)
        {
            Console.WriteLine("\nDirectory {0} has been succesfully moved to the path '{1}'\n", oldPath, newPath);
        }
    }
}
