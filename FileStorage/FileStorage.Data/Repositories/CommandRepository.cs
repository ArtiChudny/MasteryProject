using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories
{
    public class CommandRepository : ICommandInfoRepository
    {
        private readonly StorageContext _db;

        public CommandRepository(StorageContext db)
        {
            _db = db;
        }

        public Task<double> GetAverageExecutionTime(string commandName)
        {
            var execTime = _db.CommandsInfo.Where(c => c.CommandName == commandName).Average(c => c.ExecutionTime);

            return Task.FromResult(execTime);
        }

        public async Task SaveExecutionInfo(string commandName, double executionTime)
        {
            var commandExecutionInfo = new CommandExecutionInfo()
            {
                CommandName = commandName,
                ExecutionTime = executionTime
            };

            await _db.CommandsInfo.AddAsync(commandExecutionInfo);

            await _db.SaveChangesAsync();
        }
    }
}
