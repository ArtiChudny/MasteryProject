﻿using FileStorage.BLL.Models;
using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using MediatR;
using System;

namespace FileStorage.BLL.Queries
{
    public class GetDirectorySearchResultQuery : IRequest<GetDirectorySearchResultResponseModel>
    {
        public string Path { get; }
        public string SearchLine { get; }

        public GetDirectorySearchResultQuery(Options options)
        {
            if (options.Parameters.Count != 2)
            {
                throw new ArgumentException($"Wrong count of parameters '{ options.Parameters.Count}' for this command");
            }

            if (options.Flags.Count != 0)
            {
                throw new ArgumentException($"Wrong count of flags '{ options.Flags.Count}' for this command");
            }

            Path = options.Parameters[0];
            SearchLine = options.Parameters[1].ToLower();
        }
    }
}
