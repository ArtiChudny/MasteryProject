using FileStorage.DAL.Encryptors;
using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Login == login);

            if (user == null)
            {
                throw new ArgumentException($"User {login} doesn't exist");
            }

            var hashPassword = Encryptor.EncryptWithSalt(password, user.Salt);

            if (!hashPassword.SequenceEqual(user.HashPassword))
            {
                throw new ArgumentException("Wrong password");
            }

            return user.Id;
        }
    }
}
