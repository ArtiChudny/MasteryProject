using FileStorage.DAL.Models;
using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser();
    }
}
