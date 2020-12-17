using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Commands
{
    public class FileMoveCommand : IRequest
    {
        public string OldFileName { get; }
        public string NewFileName { get; }

        public FileMoveCommand(Options options)
        {
            if (options.Parameters.Count != 2)
            {
                throw new ArgumentException($"Wrong count of parameters '{options.Parameters.Count}' for this command");
            }

            if (options.Flags.Count != 0)
            {
                throw new ArgumentException($"Wrong count of flags '{options.Flags.Count}' for this command");
            }

            OldFileName = options.Parameters[0];
            NewFileName = options.Parameters[1];
        }
    }
}
