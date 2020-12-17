using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Commands
{
    public class DirectoryMoveCommand : IRequest
    {
        public string OldPath { get; }
        public string NewPath { get; }
        public DirectoryMoveCommand(Options options)
        {
            if (options.Parameters.Count != 2)
            {
                throw new ArgumentException($"Wrong count of parameters '{options.Parameters.Count}' for this command");
            }

            if (options.Flags.Count != 0)
            {
                throw new ArgumentException($"Wrong count of flags '{options.Flags.Count}' for this command");
            }

            OldPath = options.Parameters[0];
            NewPath = options.Parameters[1];
        }
    }
}
