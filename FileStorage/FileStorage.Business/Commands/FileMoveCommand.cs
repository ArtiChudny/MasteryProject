using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Commands
{
    public class FileMoveCommand : IRequest
    {
        public string OldFileName { get; set; }
        public string NewFileName { get; set; }

        public FileMoveCommand(Options options)
        {
            if (options.Parameters.Count != 2 || options.Flags.Count != 0)
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }

            OldFileName = options.Parameters[0];
            NewFileName = options.Parameters[1];
        }
    }
}
