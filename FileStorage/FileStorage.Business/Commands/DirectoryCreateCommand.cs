using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Commands
{
    public class DirectoryCreateCommand : IRequest
    {
        public string DestinationPath { get; }
        public string DirectoryName { get; }

        public DirectoryCreateCommand(Options options)
        {
            if (options.Parameters.Count != 2)
            {
                throw new ArgumentException($"Wrong count of parameters '{options.Parameters.Count}' for this command");
            }

            if (options.Flags.Count != 0)
            {
                throw new ArgumentException($"Wrong count of flags '{options.Flags.Count}' for this command");
            }

            DestinationPath = options.Parameters[0];
            DirectoryName = options.Parameters[1];
        }
    }
}
