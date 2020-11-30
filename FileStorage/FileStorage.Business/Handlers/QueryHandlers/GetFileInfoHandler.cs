﻿using FileStorage.BLL.Models.ResponceModels;
using FileStorage.BLL.Queries;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    public class GetFileInfoHandler : IRequestHandler<GetFileInfoQuery, FileInfoResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IStorageRepository _storageRepository;

        public GetFileInfoHandler(IUserRepository userRepository, IStorageRepository storageRepository)
        {
            _userRepository = userRepository;
            _storageRepository = storageRepository;
        }

        public Task<FileInfoResponseModel> Handle(GetFileInfoQuery request, CancellationToken cancellationToken)
        {
            StorageFile storageFile = _storageRepository.GetFileInfo(request.FileName);

            if (storageFile == null)
            {
                throw new ArgumentException($"File '{request.FileName}' is not exists");
            }

            User currentUser = _userRepository.GetUser();

            FileInfoResponseModel fileInfoResponseModel = new FileInfoResponseModel
            {
                FileName = request.FileName,
                Extension = storageFile.Extension,
                CreationDate = storageFile.CreationDate,
                FileSize = storageFile.Size,
                DownloadsNumber = storageFile.DownloadsNumber,
                Login = currentUser.Login
            };

            return Task.FromResult(fileInfoResponseModel);
        }
    }
}