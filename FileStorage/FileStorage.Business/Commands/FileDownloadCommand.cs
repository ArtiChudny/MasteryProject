﻿using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Commands
{
    public class FileDownloadCommand : IRequest
    {
        public string FilePath { get; set; }
        public string DestinationPath { get; set; }

        public FileDownloadCommand(Options options)
        {
            if (options.Parameters.Count != 2 || options.Flags.Count != 0)
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }

            FilePath = options.Parameters[0];
            DestinationPath = options.Parameters[1];
        }
    }
}
