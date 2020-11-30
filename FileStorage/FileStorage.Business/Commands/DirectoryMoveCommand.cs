using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Commands
{
    public class DirectoryMoveCommand : IRequest
    {
        public string OldPath { get; set; }
        public string NewPath { get; set; }
        public DirectoryMoveCommand(Options options)
        {
            if (options.Parameters.Count != 2 || options.Flags.Count != 0)
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }

            OldPath = options.Parameters[0];
            NewPath = options.Parameters[1];
        }
    }
}
