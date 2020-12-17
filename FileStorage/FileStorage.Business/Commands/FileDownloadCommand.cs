using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Commands
{
    public class FileDownloadCommand : IRequest
    {
        public string FilePath { get; }
        public string DestinationPath { get; }

        public FileDownloadCommand(Options options)
        {
            if (options.Parameters.Count != 2)
            {
                throw new ArgumentException($"Wrong count of parameters '{options.Parameters.Count}' for this command");
            }

            if (options.Flags.Count != 0)
            {
                throw new ArgumentException($"Wrong count of flags '{options.Flags.Count}' for this command");
            }

            FilePath = options.Parameters[0];
            DestinationPath = options.Parameters[1];
        }
    }
}
