using FileStorage.BLL.Commands;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.CommandHandlers
{
    class DirectoryCreateCommandHandler : IRequestHandler<DirectoryCreateCommand>
    {
        private readonly IStorageRepository _storageRepository;

        public DirectoryCreateCommandHandler(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task<Unit> Handle(DirectoryCreateCommand request, CancellationToken cancellationToken)
        {
            await _storageRepository.CreateDirectory(request.DestinationPath, request.DirectoryName);
            return Unit.Value;
        }
    }
}
