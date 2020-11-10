﻿using System;
using FileStorage.ViewModels;

namespace FileStorage
{
    public class ConsolePrinter
    {
        public void PrintUserInformation(UserInfoViewModel userInfo)
        {
            Console.WriteLine("\nlogin: {0}", userInfo.Login);
            Console.WriteLine("creation date: {0}", userInfo.CreationDate);
            Console.WriteLine("storage used: {0}\n", userInfo.UsedStorage);
        }

        public void PrintUploadSuccessful(FileUploadViewModel uploadViewModel)
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

        public void PrintDownloadSuccessful(string fileName)
        {
            Console.WriteLine("\nThe file '{0}' has been downloaded\n", fileName);
        }

        public void PrintMoveFileSuccessful(string oldFileName, string newFileName)
        {
            Console.WriteLine("\nThe file '{0}' has been moved to '{1}'\n", oldFileName, newFileName);
        }

        public void PrintRemoveSuccessful(string fileName)
        {
            Console.WriteLine("\nThe file '{0}' has been removedd\n", fileName);
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
    }
}
