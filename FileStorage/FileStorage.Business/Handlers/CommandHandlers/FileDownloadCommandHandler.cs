using FileStorage.BLL.Commands;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.CommandHandlers
{
    public class FileDownloadCommandHandler : IRequestHandler<FileDownloadCommand>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IFileRepository _fileRepository;

        public FileDownloadCommandHandler(IStorageRepository storageRepository, IFileRepository fileRepository)
        {
            _storageRepository = storageRepository;
            _fileRepository = fileRepository;
        }

        public Task<Unit> Handle(FileDownloadCommand request, CancellationToken cancellationToken)
        {
            StorageFile storageFile = _storageRepository.GetFileInfo(request.FileName);

            if (storageFile == null)
            {
                throw new ApplicationException($"File '{request.FileName}' is not exists");
            }

            if (!_fileRepository.IsHashMatch(storageFile.Id.ToString(), storageFile.Hash))
            {
                throw new ApplicationException("The file has been damaged or changed");
            }

            string storageFileName = storageFile.Id.ToString();
            _fileRepository.DownloadFileFromStorage(request.FileName, storageFileName, request.DestinationPath);
            _storageRepository.IncreaseDownloadsCounter(request.FileName);

            return Task.FromResult(Unit.Value);
        }
    }
}
