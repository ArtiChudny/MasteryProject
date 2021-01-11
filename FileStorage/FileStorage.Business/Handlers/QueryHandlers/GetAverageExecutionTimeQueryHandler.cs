using FileStorage.BLL.Queries;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    public class GetAverageExecutionTimeQueryHandler : IRequestHandler<GetAverageExecutionTimeQuery, double>
    {
        private readonly ICommandInfoRepository _commandInfoRepository;

        public GetAverageExecutionTimeQueryHandler(ICommandInfoRepository commandInfoRepository)
        {
            _commandInfoRepository = commandInfoRepository;
        }

        public async Task<double> Handle(GetAverageExecutionTimeQuery request, CancellationToken cancellationToken)
        {
            return await _commandInfoRepository.GetAverageExecutionTime(request.CommandName);
        }
    }
}
