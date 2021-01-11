using FileStorage.BLL.Commands;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.CommandHandlers
{
    public class SaveExecutionInfoCommandHandler : IRequestHandler<SaveExecutionInfoCommand>
    {
        private readonly ICommandInfoRepository _commandRepository;

        public SaveExecutionInfoCommandHandler(ICommandInfoRepository commandRepository)
        {
            _commandRepository = commandRepository;
        }

        public async Task<Unit> Handle(SaveExecutionInfoCommand request, CancellationToken cancellationToken)
        {
            await _commandRepository.SaveExecutionInfo(request.CommandName, request.ExecutionTime);

            return Unit.Value;
        }
    }
}
