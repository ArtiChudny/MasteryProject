using FileStorage.BLL.Enums;
using FileStorage.BLL.Models;
using MediatR;
using System;

namespace FileStorage.BLL.Queries
{
    public class GetFileExportFormatsQuery : IRequest<string[]>
    {
        public GetFileExportFormatsQuery(Options options)
        {
            if (options.Parameters.Count != 0 || options.Flags.Count != 1 || !options.Flags.ContainsKey(StorageFlags.Info))
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }
        }
    }
}
