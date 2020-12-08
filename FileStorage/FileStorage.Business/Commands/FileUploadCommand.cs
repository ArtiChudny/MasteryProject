using FileStorage.BLL.Models;
using FileStorage.DAL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Commands
{
    public class FileUploadCommand : IRequest<StorageFile>
    {
        public string FilePath { get; set; }
        public string DestinationDirectoryPath { get; set; }

        public FileUploadCommand(Options options)
        {
            if (options.Parameters.Count != 2 || options.Flags.Count != 0)
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }

            FilePath = options.Parameters[0];
            DestinationDirectoryPath = options.Parameters[1];
        }
    }
}
