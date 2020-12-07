using FileStorage.BLL.Commands;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.CommandHandlers
{
    public class FileMoveCommandHandler : IRequestHandler<FileMoveCommand>
    {
        private readonly IStorageRepository _storageRepository;

        public FileMoveCommandHandler(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task<Unit> Handle(FileMoveCommand request, CancellationToken cancellationToken)
        {
            await _storageRepository.MoveFile(request.OldFileName, request.NewFileName);

            return Unit.Value;
        }
    }
}
