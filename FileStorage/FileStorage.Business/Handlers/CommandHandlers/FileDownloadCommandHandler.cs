using FileStorage.BLL.Commands;
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

        public async Task<Unit> Handle(FileDownloadCommand request, CancellationToken cancellationToken)
        {
            var storageFile = await _storageRepository.GetFile(request.FilePath);

            if (storageFile == null)
            {
                throw new ApplicationException($"File '{request.FilePath}' is not exists");
            }

            if (!_fileRepository.IsHashMatch(storageFile.GuidName.ToString(), storageFile.Hash))
            {
                throw new ApplicationException("The file has been damaged or changed");
            }

            string guidFileName = storageFile.GuidName.ToString();

            await _fileRepository.DownloadFileFromStorage(storageFile.Name, guidFileName, request.DestinationPath);
            await _storageRepository.IncreaseDownloadsCounter(request.FilePath);

            return Unit.Value;
        }
    }
}
