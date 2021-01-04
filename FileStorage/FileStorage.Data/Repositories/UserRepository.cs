using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StorageContext _db;

        public UserRepository(StorageContext db)
        {
            _db = db;
        }

        public async Task<int> Authenticate(string login, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);

            if (user == null)
            {
                throw new ArgumentException("Incorrect login or password");
            }

            return user.Id;
        }
    }
}
