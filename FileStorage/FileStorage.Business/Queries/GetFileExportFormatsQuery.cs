using FileStorage.BLL.Enums;
using FileStorage.BLL.Models;
using MediatR;
using System;
using System.Linq;

namespace FileStorage.BLL.Queries
{
    public class GetFileExportFormatsQuery : IRequest<string[]>
    {
        public GetFileExportFormatsQuery(Options options)
        {
            if (options.Parameters.Count != 0)
            {
                throw new ArgumentException($"Wrong count of parameters '{options.Parameters.Count}' for this command");
            }

            if (options.Flags.Count != 1)
            {
                throw new ArgumentException($"Wrong count of flags '{options.Flags.Count}' for this command");
            }

            if (!options.Flags.ContainsKey(StorageFlags.Info))
            {
                throw new ArgumentException($"Wrong flag '--{options.Flags.Keys.First()}' for this command");
            }
        }
    }
}
