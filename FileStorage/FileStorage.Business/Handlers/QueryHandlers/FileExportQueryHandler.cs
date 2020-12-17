using FileStorage.BLL.Helpers;
using FileStorage.BLL.Queries;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    public class FileExportQueryHandler : IRequestHandler<FileExportQuery>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IFileRepository _fileRepository;

        public FileExportQueryHandler(IStorageRepository storageRepository, IFileRepository fileRepository)
        {
            _storageRepository = storageRepository;
            _fileRepository = fileRepository;
        }

        public async Task<Unit> Handle(FileExportQuery request, CancellationToken cancellationToken)
        {
            var storageInfo = await _storageRepository.GetStorageInfo();
            SerializableStorageInfo serializableStorageInfo = ConvertingHelper.GetSerializableStorageInfo(storageInfo);
            await _fileRepository.ExportFile(serializableStorageInfo, request.DestinationPath, request.Format);

            return Unit.Value;
        }
    }
}
