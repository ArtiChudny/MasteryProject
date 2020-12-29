using FileStorage.BLL.Commands;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.CommandHandlers
{
    public class FileRemoveCommandHandler : IRequestHandler<FileRemoveCommand>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IFileRepository _fileRepository;

        public FileRemoveCommandHandler(IStorageRepository storageRepository, IFileRepository fileRepository)
        {
            _storageRepository = storageRepository;
            _fileRepository = fileRepository;
        }

        public async Task<Unit> Handle(FileRemoveCommand request, CancellationToken cancellationToken)
        {
            var storageFile = await _storageRepository.GetFile(request.FilePath);

            if (storageFile == null)
            {
                throw new ApplicationException($"File '{request.FilePath}' is not exists");
            }

            var guidFileName = storageFile.GuidName.ToString();
            await _fileRepository.RemoveFile(guidFileName);
            await _storageRepository.RemoveFile(request.FilePath);

            return Unit.Value;
        }
    }
}
