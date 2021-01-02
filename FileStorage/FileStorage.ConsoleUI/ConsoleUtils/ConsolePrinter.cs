using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FileStorage.ConsoleUI.ConsoleUtils.Interfaces;
using FileStorage.ConsoleUI.ViewModels;

namespace FileStorage.ConsoleUI.ConsoleUtils
{
    public class ConsolePrinter : IConsolePrinter
    {
        public ConsolePrinter()
        {
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
        }

        public void PrintUserInformation(UserInfoViewModel userInfo)
        {
            Console.WriteLine($"\nlogin: {userInfo.Login}");
            Console.WriteLine($"creation date: {userInfo.CreationDate}");
            Console.WriteLine($"storage used: {userInfo.UsedStorage}\n");
        }

        public void PrintFileUploadedSuccessful(FileUploadViewModel uploadViewModel)
        {
            Console.WriteLine($"\nThe file '{uploadViewModel.FilePath}' has been uploaded.");
            Console.WriteLine($"- file name: '{uploadViewModel.FileName}'");
            Console.WriteLine($"- file size: '{uploadViewModel.FileSize}'");
            Console.WriteLine($"- extension: '{uploadViewModel.Extension}'\n");
        }

        public void PrintFileInfo(FileInfoViewModel fileInfoViewModel)
        {
            Console.WriteLine($"\n- file name: '{fileInfoViewModel.FileName}'");
            Console.WriteLine($"- extension: '{fileInfoViewModel.Extension}'");
            Console.WriteLine($"- file size: '{fileInfoViewModel.FileSize}'");
            Console.WriteLine($"- creation date: '{fileInfoViewModel.CreationDate}'");
            Console.WriteLine($"- downloads number: '{fileInfoViewModel.DownloadsNumber}'");
            Console.WriteLine($"- login: '{fileInfoViewModel.Login}'\n");
        }

        public void PrintFileDownloadedSuccessful(string fileName)
        {
            Console.WriteLine($"\nThe file '{fileName}' has been downloaded\n");
        }

        public void PrintFileMovedSuccessful(string oldFileName, string newFileName)
        {
            Console.WriteLine($"\nThe file '{oldFileName}' has been moved to '{newFileName}'\n");
        }

        public void PrintFileRemovedSuccessful(string fileName)
        {
            Console.WriteLine($"\nThe file '{fileName}' has been removed\n");
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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{errorMessage}\n");
            Console.ForegroundColor = ConsoleColor.White;
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
                Console.WriteLine($"- {format}");
            }
            Console.WriteLine();
        }

        public void PrintExportSuccessfull(string destinationPath)
        {
            Console.WriteLine($"\nThe meta-information has been exported, path = '{destinationPath}'\n");
        }

        public void PrintCreateDirectorySuccessfull(string destinationPath, string directoryName)
        {
            Console.WriteLine($"\nDirectory '{directoryName}' has been succesfully created at the path '{destinationPath}'\n");
        }

        public void PrintMoveDirectorySuccessfull(string oldPath, string newPath)
        {
            Console.WriteLine($"\nDirectory '{oldPath}' has been succesfully moved to the path '{newPath}'\n");
        }

        public void PrintDirectoryList(List<string> innerDirectories, List<string> innerFiles)
        {
            Console.WriteLine($"\nDirectories:{innerDirectories.Count}");
            foreach (var dir in innerDirectories)
            {
                Console.WriteLine($" - {dir}");
            }

            Console.WriteLine($"Files:{ innerFiles.Count}");
            foreach (var dir in innerFiles)
            {
                Console.WriteLine($" - {dir}");
            }
            Console.WriteLine();
        }

        public void PrintDirectoryRemovedSuccessfull(string path)
        {
            Console.WriteLine($"\nThe directory '{path}' has been removed\n");
        }

        public void PrintDirectorySearchResult(List<string> directories, List<string> files)
        {
            Console.WriteLine($"\nDirectories:{directories.Count}");
            foreach (var dir in directories)
            {
                Console.WriteLine($"- {dir}");
            }
            Console.WriteLine($"\nFiles:{files.Count}");
            foreach (var dir in files)
            {
                Console.WriteLine($"- {dir}");
            }
            Console.WriteLine();
        }

        public void PrintDirectoryInfo(DirectoryInfoViewModel directoryInfoViewModel)
        {
            Console.WriteLine($"\nName: {directoryInfoViewModel.Name}");
            Console.WriteLine($"Path: {directoryInfoViewModel.Path}");
            Console.WriteLine($"Creation date: {directoryInfoViewModel.CreationDate}");
            Console.WriteLine($"Modification date: {directoryInfoViewModel.ModificationDate}");
            Console.WriteLine($"Size: { directoryInfoViewModel.Size}");
            Console.WriteLine($"Login: {directoryInfoViewModel.Login}\n");
        }

        public async void PrintLoading(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.Write("Loading");
                for (int i = 0; i < 3; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        ClearCurrentConsoleLine();
                        return;
                    }
                    await Task.Delay(100);
                    Console.Write(".");
                }
                ClearCurrentConsoleLine();
            }
        }

        public void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

    }
}

