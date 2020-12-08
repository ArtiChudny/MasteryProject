using FileStorage.BLL.Commands;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System;
using System.IO;
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
            var storageFile = await _storageRepository.GetFileInfo(request.FilePath);

            if (storageFile == null)
            {
                throw new ApplicationException($"File '{request.FilePath}' is not exists");
            }

            if (!_fileRepository.IsHashMatch(storageFile.Id.ToString(), storageFile.Hash))
            {
                throw new ApplicationException("The file has been damaged or changed");
            }

            string fileName = Path.GetFileName(request.FilePath);
            string guidFileName = storageFile.Id.ToString();

            await _fileRepository.DownloadFileFromStorage(fileName, guidFileName, request.DestinationPath);
            _storageRepository.IncreaseDownloadsCounter(request.FilePath);

            return Unit.Value;
        }
    }
}
