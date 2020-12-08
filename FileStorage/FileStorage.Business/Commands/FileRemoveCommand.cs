using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Commands
{
    public class FileRemoveCommand : IRequest
    {
        public string FilePath { get; set; }
        public FileRemoveCommand(Options options)
        {
            if (options.Parameters.Count != 1 || options.Flags.Count != 0)
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }

            FilePath = options.Parameters[0];
        }
    }
}
