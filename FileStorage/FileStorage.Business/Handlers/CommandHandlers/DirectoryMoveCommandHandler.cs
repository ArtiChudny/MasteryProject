using FileStorage.BLL.Commands;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.CommandHandlers
{
    class DirectoryMoveCommandHandler : IRequestHandler<DirectoryMoveCommand>
    {
        private readonly IStorageRepository _storageRepository;

        public DirectoryMoveCommandHandler(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public Task<Unit> Handle(DirectoryMoveCommand request, CancellationToken cancellationToken)
        {
            _storageRepository.MoveDirectory(request.OldPath, request.NewPath);

            return Task.FromResult(Unit.Value);
        }
    }
}
