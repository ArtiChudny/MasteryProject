using FileStorage.DAL.Constants;
using FileStorage.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.DAL
{
    public class StorageContext : DbContext
    {
        public DbSet<StorageDirectory> Directories { get; set; }
        public DbSet<StorageFile> Files { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(e =>
            {
                e.HasMany(u => u.Directories);
                e.Property(p => p.Login).IsRequired();
                e.Property(p => p.Password).IsRequired();
                e.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()");
                e.HasData(new User()
                {
                    Id = 1,
                    Login = "StorageUser",
                    Password = "StoragePassword",
                });
            });

            modelBuilder.Entity<StorageDirectory>(e =>
            {
                e.HasOne(d => d.ParentDirectory).WithMany(p => p.Directories).HasForeignKey(f => f.ParentId);
                e.HasMany(d => d.Files).WithOne(f => f.ParentDirectory).HasForeignKey(f => f.DirectoryId);
                e.Property(p => p.Name).IsRequired();
                e.Property(p => p.Path).IsRequired();
                e.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()");
                e.Property(p => p.ModificationDate).HasDefaultValueSql("GETDATE()");
                e.HasData(new StorageDirectory()
                {
                    Id = 1,
                    Name = DirectoryPaths.InitialDirectoryPath.Replace("/", ""),
                    Path = DirectoryPaths.InitialDirectoryPath,
                    UserId = 1
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
        }
    }
}
