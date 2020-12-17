﻿using FileStorage.BLL.Models;
using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using MediatR;
using System;

namespace FileStorage.BLL.Queries
{
    public class GetDirectoryInnerListQuery : IRequest<GetDirectoryInnerListResponseModel>
    {
        public string Path { get; }

        public GetDirectoryInnerListQuery(Options options)
        {
            if (options.Parameters.Count != 1)
            {
                throw new ArgumentException($"Wrong count of parameters '{ options.Parameters.Count}' for this command");
            }

            if (options.Flags.Count != 0)
            {
                throw new ArgumentException($"Wrong count of flags '{ options.Flags.Count}' for this command");
            }

            Path = options.Parameters[0];
        }
    }
}
