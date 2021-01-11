using MediatR;

namespace FileStorage.BLL.Queries
{
    public class GetAverageExecutionTimeQuery : IRequest<double>
    {
        public string CommandName { get; set; }

        public GetAverageExecutionTimeQuery(string commandName)
        {
            CommandName = commandName;
        }
    }
}
