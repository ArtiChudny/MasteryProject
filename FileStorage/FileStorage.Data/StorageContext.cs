using FileStorage.DAL.Constants;
using FileStorage.DAL.Encryptors;
using FileStorage.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace FileStorage.DAL
{
    public class StorageContext : DbContext
    {
        private readonly StreamWriter _logStream = new StreamWriter($"{ConfigurationManager.AppSettings["LogPath"]}/{DateTime.Today:yyy-MM-dd} EfLog.txt", true);

        public DbSet<StorageDirectory> Directories { get; set; }
        public DbSet<StorageFile> Files { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.HasMany(u => u.Directories);
                e.Property(p => p.Login).IsRequired();
                e.Property(p => p.HashPassword).IsRequired();
                e.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()");

                var password1 = Encryptor.Encrypt("firstPassword");
                var password2 = Encryptor.Encrypt("secondPassword");

                e.HasData(
                    new User()
                    {
                        Id = 1,
                        Login = "firstUser",
                        HashPassword = password1.HashPassword,
                        Salt = password1.Salt
                    },
                    new User()
                    {
                        Id = 2,
                        Login = "secondUser",
                        HashPassword = password2.HashPassword,
                        Salt = password2.Salt
                    }); ;
            });

            modelBuilder.Entity<StorageDirectory>(e =>
            {
                e.HasOne(d => d.ParentDirectory).WithMany(p => p.Directories).HasForeignKey(f => f.ParentId);
                e.HasMany(d => d.Files).WithOne(f => f.ParentDirectory).HasForeignKey(f => f.DirectoryId);
                e.Property(p => p.Name).IsRequired();
                e.Property(p => p.Path).IsRequired();
                e.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()");
                e.Property(p => p.ModificationDate).HasDefaultValueSql("GETDATE()");
                e.HasData(
                    new StorageDirectory()
                    {
                        Id = 1,
                        Name = DirectoryPaths.InitialDirectoryPath.Replace("/", ""),
                        Path = DirectoryPaths.InitialDirectoryPath,
                        UserId = 1
                    },
                    new StorageDirectory()
                    {
                        Id = 2,
                        Name = DirectoryPaths.InitialDirectoryPath.Replace("/", ""),
                        Path = DirectoryPaths.InitialDirectoryPath,
                        UserId = 2
                    });
            });

            modelBuilder.Entity<StorageFile>(e =>
            {
                e.Property(p => p.Name).IsRequired();
                e.Property(p => p.Path).IsRequired();
                e.Property(p => p.Hash).IsRequired();
                e.Property(p => p.Extension).IsRequired();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=A-Chudny\SQLEXPRESS;Database=StorageRepository;Trusted_Connection=True;");
            optionsBuilder.LogTo(_logStream.WriteLine).EnableSensitiveDataLogging();
        }

        public override void Dispose()
        {
            base.Dispose();
            _logStream.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _logStream.DisposeAsync();
        }
    }
}
