using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories.Interfaces
{
    public interface ICommandInfoRepository
    {
        public Task SaveExecutionInfo(string commandName, double executionTime);
        public Task<double> GetAverageExecutionTime(string commandName);
    }
}
