using FileStorage.ConsoleUI.ViewModels;
using FileStorage.ConsoleUI.Helpers;
using FileStorage.ConsoleUI.ConsoleUtils.Interfaces;
using Microsoft.Extensions.Logging;
using MediatR;
using FileStorage.BLL.Queries;
using FileStorage.BLL.Models;
using FileStorage.BLL.Commands;
using System.IO;
using FileStorage.BLL.Enums;

public class Controller
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

    public void ExecuteConsoleCommand(StorageCommand command)
    {
        switch (command.CommandType)
        {
            case StorageCommands.UserInfo:
                {
                    ExecuteCommandGetUserInfo(command.Options);
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
            case StorageCommands.DirectoryCreate:
                {
                    ExecuteCommandDirectoryCreate(command.Options);
                    break;
                }
            case StorageCommands.DirectoryMove:
                {
                    ExecuteCommandDirectoryMove(command.Options);
                    break;
                }
        }
    }

    private void ExecuteCommandGetUserInfo(Options options)
    {
        var query = new GetUserInfoQuery(options);
        var result = _mediator.Send(query).Result;

        var userInfoViewModel = new UserInfoViewModel
        {
            CreationDate = ConvertingHelper.GetDateString(result.CreationDate),
            Login = result.Login,
            UsedStorage = ConvertingHelper.GetSizeString(result.UsedStorage)
        };

        _consolePrinter.PrintUserInformation(userInfoViewModel);
    }

    private void ExecuteCommandFileUpload(Options options)
    {
        var command = new FileUploadCommand(options);
        var result = _mediator.Send(command).Result;

        FileUploadViewModel uploadViewModel = new FileUploadViewModel
        {
            FilePath = command.FilePath,
            FileName = Path.GetFileName(command.FilePath),
            FileSize = ConvertingHelper.GetSizeString(result.Size),
            Extension = result.Extension
        };

        LogInformationMessage($"File \"{command.FilePath}\" has been uploaded");
        _consolePrinter.PrintFileUploadedSuccessful(uploadViewModel);
    }

    private void ExecuteCommandFileDownload(Options options)
    {
        var command = new FileDownloadCommand(options);
        _mediator.Send(command);

        LogInformationMessage($"File \"{command.FileName}\" has been downloaded to {command.DestinationPath}");
        _consolePrinter.PrintFileDownloadedSuccessful(command.FileName);
    }

    private void ExecuteCommandFileMove(Options options)
    {
        var command = new FileMoveCommand(options);
        _mediator.Send(command);

        LogInformationMessage($"File \"{command.OldFileName}\" has been moved to {command.NewFileName}");
        _consolePrinter.PrintFileMovedSuccessful(command.OldFileName, command.NewFileName);
    }

    private void ExecuteCommandFileRemove(Options options)
    {
        var command = new FileRemoveCommand(options);
        _mediator.Send(command);

        LogInformationMessage($"File \"{command.FileName}\" has been removed");
        _consolePrinter.PrintFileRemovedSuccessful(command.FileName);
    }

    private void ExecuteCommandFileInfo(Options options)
    {
        var query = new GetFileInfoQuery(options);
        var result = _mediator.Send(query).Result;

        FileInfoViewModel fileInfoViewModel = new FileInfoViewModel
        {
            FileName = result.FileName,
            Extension = result.Extension,
            CreationDate = ConvertingHelper.GetDateString(result.CreationDate),
            FileSize = ConvertingHelper.GetSizeString(result.FileSize),
            DownloadsNumber = result.DownloadsNumber,
            Login = result.Login
        };

        _consolePrinter.PrintFileInfo(fileInfoViewModel);
    }

    private void ExecuteCommandFileExport(Options options)
    {
        if (options.Parameters.Count == 0)
        {
            var query = new GetFileExportFormatsQuery(options);
            var result = _mediator.Send(query).Result;
            _consolePrinter.PrintExportFormats(result);
        }

        if (options.Parameters.Count == 1)
        {
            var command = new FileExportCommand(options);
            _mediator.Send(command);

            LogInformationMessage($"Meta-info file has been exported to '{command.DestinationPath}'");
            _consolePrinter.PrintExportSuccessfull(command.DestinationPath);
        }
    }

    private void ExecuteCommandDirectoryCreate(Options options)
    {
        var command = new DirectoryCreateCommand(options);
        _mediator.Send(command);

        LogInformationMessage($"Directory {command.DirectoryName} has been succesfully created at the path '{command.DestinationPath}'");
        _consolePrinter.PrintCreateDirectorySuccessfull(command.DestinationPath, command.DirectoryName);
    }

    private void ExecuteCommandDirectoryMove(Options options)
    {
        var command = new DirectoryMoveCommand(options);
        _mediator.Send(command);

        LogInformationMessage($"Directory {command.OldPath} has been succesfully moved to the path '{command.NewPath}");
        _consolePrinter.PrintMoveDirectorySuccessfull(command.OldPath, command.NewPath);
    }

    private void LogInformationMessage(string message)
    {
        string logMessage = ConvertingHelper.GetLogMessage(message, string.Empty);
        _logger.LogInformation(logMessage);
    }
}
