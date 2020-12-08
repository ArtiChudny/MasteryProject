using FileStorage.BLL.Models;
using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using MediatR;
using System;

namespace FileStorage.BLL.Queries
{
    public class GetDirectorySearchResultQuery : IRequest<GetDirectorySearchResultResponseModel>
    {
        public string Path { get; set; }
        public string SearchLine { get; set; }

        public GetDirectorySearchResultQuery(Options options)
        {
            if (options.Parameters.Count != 2 || options.Flags.Count != 0)
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }

            Path = options.Parameters[0];
            SearchLine = options.Parameters[1].ToLower();
        }
    }
}
