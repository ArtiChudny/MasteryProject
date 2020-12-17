using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Commands
{
    public class FileRemoveCommand : IRequest
    {
        public string FilePath { get; }
        public FileRemoveCommand(Options options)
        {
            if (options.Parameters.Count != 1)
            {
                throw new ArgumentException($"Wrong count of parameters '{ options.Parameters.Count}' for this command");
            }

            if (options.Flags.Count != 0)
            {
                throw new ArgumentException($"Wrong count of flags '{ options.Flags.Count}' for this command");
            }

            FilePath = options.Parameters[0];
        }
    }
}
