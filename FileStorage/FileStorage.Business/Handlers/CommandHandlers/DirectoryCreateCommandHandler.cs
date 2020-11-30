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

        public Task<Unit> Handle(DirectoryCreateCommand request, CancellationToken cancellationToken)
        {
            _storageRepository.CreateDirectory(request.DestinationPath, request.DirectoryName);

            return Task.FromResult(Unit.Value);
        }
    }
}
