﻿using FileStorage.BLL.Models;
using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using MediatR;
using System;

namespace FileStorage.BLL.Queries
{
    public class GetDirectoryInfoQuery : IRequest<DirectoryInfoResponseModel>
    {
        public string Path { get; set; }

        public GetDirectoryInfoQuery(Options options)
        {
            if (options.Parameters.Count != 1 || options.Flags.Count != 0)
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }

            Path = options.Parameters[0];
        }
    }
}