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

        public async Task<Unit> Handle(DirectoryMoveCommand request, CancellationToken cancellationToken)
        {
            await _storageRepository.MoveDirectory(request.OldPath, request.NewPath);

            return Unit.Value;
        }
    }
}
