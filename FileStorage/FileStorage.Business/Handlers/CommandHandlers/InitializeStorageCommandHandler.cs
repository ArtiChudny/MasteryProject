using FileStorage.BLL.Commands;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.CommandHandlers
{
    public class InitializeStorageCommandHandler : IRequestHandler<InitializeStorageCommand>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IFileRepository _fileRepository;

        public InitializeStorageCommandHandler(IStorageRepository storageRepository, IFileRepository fileRepository)
        {
            _storageRepository = storageRepository;
            _fileRepository = fileRepository;
        }

        public Task<Unit> Handle(InitializeStorageCommand request, CancellationToken cancellationToken)
        {
            _storageRepository.InitializeStorage();
            _fileRepository.InitializeFileStorage();

            return Task.FromResult(Unit.Value);
        }
    }
}
