using FileStorage.DAL.Constants;
using FileStorage.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.DAL
{
    public class StorageContext : DbContext
    {
        public DbSet<StorageDirectory> Directories { get; set; }
        public DbSet<StorageFile> Files { get; set; }

        public StorageContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
