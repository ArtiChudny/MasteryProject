using FileStorage.BLL.Models;
using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using MediatR;
using System;

namespace FileStorage.BLL.Queries
{
    public class GetFileInfoQuery : IRequest<FileInfoResponseModel>
    {
        public string FileName { get; set; }
        public GetFileInfoQuery(Options options)
        {
            if (options.Parameters.Count != 1 || options.Flags.Count != 0)
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }

            FileName = options.Parameters[0];
        }
    }
}
