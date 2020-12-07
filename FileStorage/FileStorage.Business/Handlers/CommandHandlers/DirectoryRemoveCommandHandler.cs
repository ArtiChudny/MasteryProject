using FileStorage.BLL.Commands;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.CommandHandlers
{
    public class DirectoryRemoveCommandHandler : IRequestHandler<DirectoryRemoveCommand>
    {
        private readonly IStorageRepository _storageRepository;

        public DirectoryRemoveCommandHandler(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task<Unit> Handle(DirectoryRemoveCommand request, CancellationToken cancellationToken)
        {
            await _storageRepository.RemoveDirectory(request.Path);

            return Unit.Value;
        }
    }
}
