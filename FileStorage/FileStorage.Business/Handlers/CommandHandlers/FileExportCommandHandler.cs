using FileStorage.BLL.Commands;
using FileStorage.BLL.Helpers;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.CommandHandlers
{
    public class FileExportCommandHandler : IRequestHandler<FileExportCommand>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IFileRepository _fileRepository;

        public FileExportCommandHandler(IStorageRepository storageRepository, IFileRepository fileRepository)
        {
            _storageRepository = storageRepository;
            _fileRepository = fileRepository;
        }

        public Task<Unit> Handle(FileExportCommand request, CancellationToken cancellationToken)
        {
            StorageInfo storageInfo = _storageRepository.GetStorageInfo();
            SerializableStorageInfo serializableStorageInfo = ConvertingHelper.GetSerializableStorageInfo(storageInfo);
            _fileRepository.ExportFile(serializableStorageInfo, request.DestinationPath, request.Format);

            return Task.FromResult(Unit.Value);
        }
    }
}
