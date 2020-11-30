using FileStorage.BLL.Models;
using FileStorage.BLL.Models.ResponceModels;
using MediatR;
using System;

namespace FileStorage.BLL.Queries
{
    public class GetUserInfoQuery : IRequest<UserInfoResponseModel>
    {
        public GetUserInfoQuery(Options options)
        {
            if (options.Parameters.Count != 0 || options.Flags.Count != 0)
            {
                throw new ArgumentException("Wrong count of parameters or flags for this command");
            }
        }
    }
}
