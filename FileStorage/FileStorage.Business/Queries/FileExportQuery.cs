using FileStorage.BLL.Enums;
using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Queries
{
    public class FileExportQuery : IRequest
    {
        public string DestinationPath { get; set; }
        public string Format { get; set; }

        public FileExportQuery(Options options)
        {
            if (options.Parameters.Count != 1 || options.Flags.Count > 1)
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }

            DestinationPath = options.Parameters[0];

            if (options.Flags.ContainsKey(StorageFlags.Format))
            {
                Format = options.Flags[StorageFlags.Format];
            }
        }
    }
}
