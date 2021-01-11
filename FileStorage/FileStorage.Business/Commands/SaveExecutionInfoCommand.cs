using MediatR;

namespace FileStorage.BLL.Commands
{
    public class SaveExecutionInfoCommand : IRequest
    {
        public string CommandName { get; set; }
        public double ExecutionTime { get; set; }

        public SaveExecutionInfoCommand(string commandName, double executionTime)
        {
            CommandName = commandName;
            ExecutionTime = executionTime;
        }
    }
}
