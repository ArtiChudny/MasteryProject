using FileStorage.ConsoleUI.ViewModels;
using FileStorage.ConsoleUI.Helpers;
using FileStorage.ConsoleUI.ConsoleUtils.Interfaces;
using Microsoft.Extensions.Logging;
using MediatR;
using FileStorage.BLL.Queries;
using FileStorage.BLL.Models;
using FileStorage.BLL.Commands;
using FileStorage.BLL.Enums;
using System.IO;
using System.Threading.Tasks;
using System;

namespace FileStorage.ConsoleUI.Controllers
{
    public class Controller : IController
    {
        private readonly IConsolePrinter _consolePrinter;
        private readonly ILogger<Controller> _logger;
        private readonly IMediator _mediator;

        public Controller(IConsolePrinter consolePrinter, ILogger<Controller> logger, IMediator mediator)
        {
            _consolePrinter = consolePrinter;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task ExecuteConsoleCommand(StorageCommand command)
        {
            switch (command.CommandType)
            {
                case StorageCommands.UserInfo:
                    {
                        await ExecuteConsoleCommandGetUserInfo(command.Options);
                        break;
                    }
                case StorageCommands.FileUpload:
                    {
                        await ExecuteConsoleCommandFileUpload(command.Options);
                        break;
                    }
                case StorageCommands.FileDownload:
                    {
                        await ExecuteConsoleCommandFileDownload(command.Options);
                        break;
                    }
                case StorageCommands.FileMove:
                    {
                        await ExecuteConsoleCommandFileMove(command.Options);
                        break;
                    }
                case StorageCommands.FileRemove:
                    {
                        await ExecuteConsoleCommandFileRemove(command.Options);
                        break;
                    }
                case StorageCommands.FileInfo:
                    {
                        await ExecuteConsoleCommandFileInfo(command.Options);
                        break;
                    }
                case StorageCommands.FileExport:
                    {
                        await ExecuteConsoleCommandFileExport(command.Options);
                        break;
                    }
                case StorageCommands.DirectoryCreate:
                    {
                        await ExecuteConsoleCommandDirectoryCreate(command.Options);
                        break;
                    }
                case StorageCommands.DirectoryMove:
                    {
                        await ExecuteConsoleCommandDirectoryMove(command.Options);
                        break;
                    }
                case StorageCommands.DirectoryRemove:
                    {
                        await ExecuteConsoleCommandDirectoryRemove(command.Options);
                        break;
                    }
                case StorageCommands.DirectoryList:
                    {
                        await ExecuteConsoleCommandDirectoryList(command.Options);
                        break;
                    }
                case StorageCommands.DirectorySearch:
                    {
                        await ExecuteConsoleCommandDirectorySearch(command.Options);
                        break;
                    }
                case StorageCommands.DirectoryInfo:
                    {
                        await ExecuteConsoleCommandDirectoryInfo(command.Options);
                        break;
                    }
            }
        }

        private async Task ExecuteConsoleCommandGetUserInfo(Options options)
        {
            var getUserQuery = new GetUserInfoQuery(options);
            var userInfoResponceModel = await _mediator.Send(getUserQuery);

            var userInfoViewModel = new UserInfoViewModel
            {
                CreationDate = ConvertingHelper.GetDateString(userInfoResponceModel.CreationDate),
                Login = userInfoResponceModel.Login,
                UsedStorage = ConvertingHelper.GetSizeString(userInfoResponceModel.UsedStorage)
            };

            _consolePrinter.PrintUserInformation(userInfoViewModel);
        }

        private async Task ExecuteConsoleCommandFileUpload(Options options)
        {
            var fileUploadCommand = new FileUploadCommand(options);
            var storageFile = await _mediator.Send(fileUploadCommand);

            FileUploadViewModel uploadViewModel = new FileUploadViewModel
            {
                FilePath = fileUploadCommand.FilePath,
                FileName = Path.GetFileName(fileUploadCommand.FilePath),
                FileSize = ConvertingHelper.GetSizeString(storageFile.Size),
                Extension = storageFile.Extension
            };

            LogInformationMessage($"File \"{fileUploadCommand.FilePath}\" has been uploaded");
            _consolePrinter.PrintFileUploadedSuccessful(uploadViewModel);
        }

        private async Task ExecuteConsoleCommandFileDownload(Options options)
        {
            var fileDownloadCommand = new FileDownloadCommand(options);
            await _mediator.Send(fileDownloadCommand);

            LogInformationMessage($"File \"{fileDownloadCommand.FilePath}\" has been downloaded to {fileDownloadCommand.DestinationPath}");
            _consolePrinter.PrintFileDownloadedSuccessful(fileDownloadCommand.FilePath);
        }

        private async Task ExecuteConsoleCommandFileMove(Options options)
        {
            var fileMoveCommand = new FileMoveCommand(options);
            await _mediator.Send(fileMoveCommand);

            LogInformationMessage($"File \"{fileMoveCommand.OldFileName}\" has been moved to {fileMoveCommand.NewFileName}");
            _consolePrinter.PrintFileMovedSuccessful(fileMoveCommand.OldFileName, fileMoveCommand.NewFileName);
        }

        private async Task ExecuteConsoleCommandFileRemove(Options options)
        {
            var fileRemoveCommand = new FileRemoveCommand(options);
            await _mediator.Send(fileRemoveCommand);

            LogInformationMessage($"File \"{fileRemoveCommand.FilePath}\" has been removed");
            _consolePrinter.PrintFileRemovedSuccessful(fileRemoveCommand.FilePath);
        }

        private async Task ExecuteConsoleCommandFileInfo(Options options)
        {
            var fileInfoQuery = new GetFileInfoQuery(options);
            var fileInfoResponseModel = await _mediator.Send(fileInfoQuery);

            FileInfoViewModel fileInfoViewModel = new FileInfoViewModel
            {
                FileName = fileInfoResponseModel.FileName,
                Extension = fileInfoResponseModel.Extension,
                CreationDate = ConvertingHelper.GetDateString(fileInfoResponseModel.CreationDate),
                FileSize = ConvertingHelper.GetSizeString(fileInfoResponseModel.FileSize),
                DownloadsNumber = fileInfoResponseModel.DownloadsNumber,
                Login = fileInfoResponseModel.Login
            };

            _consolePrinter.PrintFileInfo(fileInfoViewModel);
        }

        private async Task ExecuteConsoleCommandFileExport(Options options)
        {
            if (options.Parameters.Count == 0)
            {
                var getFileExportFormatsQuery = new GetFileExportFormatsQuery(options);
                var formatsArray = await _mediator.Send(getFileExportFormatsQuery);
                _consolePrinter.PrintExportFormats(formatsArray);
            }

            if (options.Parameters.Count == 1)
            {
                var fileExportCommand = new FileExportQuery(options);
                await _mediator.Send(fileExportCommand);

                LogInformationMessage($"Meta-info file has been exported to '{fileExportCommand.DestinationPath}'");
                _consolePrinter.PrintExportSuccessfull(fileExportCommand.DestinationPath);
            }
        }

        private async Task ExecuteConsoleCommandDirectoryCreate(Options options)
        {
            var directoryCreateCommand = new DirectoryCreateCommand(options);
            await _mediator.Send(directoryCreateCommand);

            LogInformationMessage($"Directory {directoryCreateCommand.DirectoryName} has been succesfully created at the path '{directoryCreateCommand.DestinationPath}'");
            _consolePrinter.PrintCreateDirectorySuccessfull(directoryCreateCommand.DestinationPath, directoryCreateCommand.DirectoryName);
        }

        private async Task ExecuteConsoleCommandDirectoryMove(Options options)
        {
            var directoryMoveCommand = new DirectoryMoveCommand(options);
            await _mediator.Send(directoryMoveCommand);

            LogInformationMessage($"Directory {directoryMoveCommand.OldPath} has been moved to the path '{directoryMoveCommand.NewPath}");
            _consolePrinter.PrintMoveDirectorySuccessfull(directoryMoveCommand.OldPath, directoryMoveCommand.NewPath);
        }

        private async Task ExecuteConsoleCommandDirectoryRemove(Options options)
        {
            var directoryRemoveCommand = new DirectoryRemoveCommand(options);
            await _mediator.Send(directoryRemoveCommand);

            LogInformationMessage($"The directory {directoryRemoveCommand.Path} has been removed");
            _consolePrinter.PrintDirectoryRemovedSuccessfull(directoryRemoveCommand.Path);
        }

        private async Task ExecuteConsoleCommandDirectoryList(Options options)
        {
            var getDirectoryQuery = new GetDirectoryInnerListQuery(options);
            var directoryInnerList = await _mediator.Send(getDirectoryQuery);

            _consolePrinter.PrintDirectoryList(directoryInnerList.InnerDirectories, directoryInnerList.InnerFiles);
        }

        private async Task ExecuteConsoleCommandDirectorySearch(Options options)
        {
            var getDirectorySearchResultQuery = new GetDirectorySearchResultQuery(options);
            var searchResult = await _mediator.Send(getDirectorySearchResultQuery);

            _consolePrinter.PrintDirectorySearchResult(searchResult.MatchedDirectories, searchResult.MatchedFiles);
        }

        private async Task ExecuteConsoleCommandDirectoryInfo(Options options)
        {
            var getDirectoryInfoQuery = new GetDirectoryInfoQuery(options);
            var directoryInfo = await _mediator.Send(getDirectoryInfoQuery);

            var directoryInfoViewModel = new DirectoryInfoViewModel()
            {
                CreationDate = ConvertingHelper.GetDateString(directoryInfo.CreationDate),
                ModificationDate = ConvertingHelper.GetDateString(directoryInfo.ModificationDate),
                Login = directoryInfo.Login,
                Name = directoryInfo.Name,
                Path = directoryInfo.Path,
                Size = ConvertingHelper.GetSizeString(directoryInfo.Size)
            };

            _consolePrinter.PrintDirectoryInfo(directoryInfoViewModel);
        }

        private void LogInformationMessage(string message)
        {
            string logMessage = ConvertingHelper.GetLogMessage(message, string.Empty);
            _logger.LogInformation(logMessage);
        }
    }
}