using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Commands
{
    public class DirectoryCreateCommand : IRequest
    {
        public string DestinationPath { get; set; }
        public string DirectoryName { get; set; }

        public DirectoryCreateCommand(Options options)
        {
            if (options.Parameters.Count != 2 || options.Flags.Count != 0)
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }

            DestinationPath = options.Parameters[0];
            DirectoryName = options.Parameters[1];
        }

    }
}
