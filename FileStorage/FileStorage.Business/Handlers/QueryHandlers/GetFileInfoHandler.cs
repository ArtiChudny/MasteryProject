using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using FileStorage.BLL.Queries;
using FileStorage.DAL;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    public class GetFileInfoHandler : IRequestHandler<GetFileInfoQuery, FileInfoResponseModel>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly CurrentUser _currentUser;

        public GetFileInfoHandler(IStorageRepository storageRepository, CurrentUser currentUser)
        {
            _storageRepository = storageRepository;
            _currentUser = currentUser;
        }

        public async Task<FileInfoResponseModel> Handle(GetFileInfoQuery request, CancellationToken cancellationToken)
        {
            var storageFile = await _storageRepository.GetFile(request.FileName);

            if (storageFile == null)
            {
                throw new ArgumentException($"File '{request.FileName}' is not exists");
            }

            var fileInfoResponseModel = new FileInfoResponseModel
            {
                FileName = storageFile.Name,
                Extension = storageFile.Extension,
                CreationDate = storageFile.CreationDate,
                FileSize = storageFile.Size,
                DownloadsNumber = storageFile.DownloadsNumber,
                Login = _currentUser.Login
            };

            return fileInfoResponseModel;
        }
    }
}
