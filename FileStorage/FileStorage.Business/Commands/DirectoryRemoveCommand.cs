using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Commands
{
    public class DirectoryRemoveCommand : IRequest
    {
        public string Path { get; set; }

        public DirectoryRemoveCommand(Options options)
        {
            if (options.Parameters.Count != 1 || options.Flags.Count != 0)
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }

            Path = options.Parameters[0];
        }
    }
}
