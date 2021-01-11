using FileStorage.DAL.Encryptors;
using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StorageContext _db;

        public UserRepository(StorageContext db, ILoggerProvider loggerProvider)
        {
            _db = db;
            _db.GetService<ILoggerFactory>().AddProvider(loggerProvider);
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
