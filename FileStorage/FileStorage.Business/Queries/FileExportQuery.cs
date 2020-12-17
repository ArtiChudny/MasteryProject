using FileStorage.BLL.Enums;
using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Queries
{
    public class FileExportQuery : IRequest
    {
        public string DestinationPath { get; }
        public string Format { get; }

        public FileExportQuery(Options options)
        {
            if (options.Parameters.Count != 1)
            {
                throw new ArgumentException($"Wrong count of parameters '{options.Parameters.Count}' for this command");
            }

            if (options.Flags.Count > 1)
            {
                throw new ArgumentException($"Wrong count of flags '{options.Flags.Count}' for this command");
            }

            DestinationPath = options.Parameters[0];

            if (options.Flags.ContainsKey(StorageFlags.Format))
            {
                Format = options.Flags[StorageFlags.Format];
            }
        }
    }
}
