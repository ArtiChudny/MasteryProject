using FileStorage.BLL.Commands;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.CommandHandlers
{
    public class DirectoryRemoveCommandHandler : IRequestHandler<DirectoryRemoveCommand>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IFileRepository _fileRepository;

        public DirectoryRemoveCommandHandler(IStorageRepository storageRepository, IFileRepository fileRepository)
        {
            _storageRepository = storageRepository;
            _fileRepository = fileRepository;
        }

        public async Task<Unit> Handle(DirectoryRemoveCommand request, CancellationToken cancellationToken)
        {
            var directory = await _storageRepository.GetDirectory(request.Path);

            RemovePhysicalFiles(directory);
            await _storageRepository.RemoveDirectory(request.Path);

            return Unit.Value;
        }

        //The method recursively delete the physical files from storage folder
        private void RemovePhysicalFiles(StorageDirectory directory)
        {
            foreach (var file in directory.Files)
            {
                _fileRepository.RemoveFile(file.GuidName.ToString());
            }

            foreach (var dir in directory.Directories)
            {
                RemovePhysicalFiles(dir);
            }
        }
    }
}
