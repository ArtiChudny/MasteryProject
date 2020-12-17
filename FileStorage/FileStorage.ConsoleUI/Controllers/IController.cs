using FileStorage.BLL.Models;
using System.Threading.Tasks;

namespace FileStorage.ConsoleUI.Controllers
{
    public interface IController
    {
        Task ExecuteConsoleCommand(StorageCommand command);
    }
}