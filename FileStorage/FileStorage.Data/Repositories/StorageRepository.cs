using FileStorage.DAL.Constants;
using FileStorage.DAL.Helpers;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private readonly StorageContext _db;
        private readonly CurrentUser _currentUser;

        public StorageRepository(StorageContext db, CurrentUser currentUser, ILoggerProvider loggerProvider)
        {
            _db = db;
            _currentUser = currentUser;
            _db.GetService<ILoggerFactory>().AddProvider(loggerProvider); 
        }

        public async Task CreateDirectory(string path, string directoryName)
        {
            var parentDirrectory = await GetDirectory(path);

            if (parentDirrectory.Directories.Any(d => d.Name == directoryName))
            {
                throw new ArgumentException($"The directory {directoryName} is already exists at the path '{path}'");
            }

            parentDirrectory.Directories.Add(new StorageDirectory()
            {
                Name = directoryName,
                ParentId = parentDirrectory.Id,
                Path = $"{parentDirrectory.Path}/{directoryName}",
                UserId = _currentUser.Id
            });

            parentDirrectory.ModificationDate = DateTime.Now;

            await _db.SaveChangesAsync();
        }

        public async Task<StorageFile> CreateFile(string directoryPath, string fileName, long fileSize, byte[] hash, DateTime creationDate)
        {
            var parentDirectory = await GetDirectory(directoryPath);

            if (parentDirectory.Files.Any(f => f.Name == fileName))
            {
                throw new ArgumentException($"The file '{fileName}' is already exists in the '{directoryPath}'");
            }

            var storageFile = new StorageFile
            {
                Name = fileName,
                GuidName = Guid.NewGuid(),
                CreationDate = creationDate,
                Extension = Path.GetExtension(fileName),
                Size = fileSize,
                Hash = hash,
                Path = $"{parentDirectory.Path}/{fileName}",
                DownloadsNumber = 0
            };

            parentDirectory.Files.Add(storageFile);
            parentDirectory.ModificationDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return storageFile;
        }

        public async Task<StorageDirectory> GetDirectory(string path)
        {
            var directory = await _db.Directories
                                                .Include(d => d.ParentDirectory)
                                                .Include(d => d.Files)
                                                .Include(d => d.Directories)
                                                .FirstOrDefaultAsync(d => d.Path == path && d.UserId == _currentUser.Id);

            if (directory == null)
            {
                throw new ArgumentException($"The directory {path} doesn't exist");
            }

            return directory;
        }

        public async Task<StorageDirectory> GetFullDirectoryTree(string path)
        {
            var directory = await GetDirectory(path);
            FillDirectoryTree(directory);

            return directory;
        }

        public async Task<StorageFile> GetFile(string filePath)
        {
            var storageFile = await _db.Files.Include(d => d.ParentDirectory).FirstOrDefaultAsync(f => f.Path == filePath && f.ParentDirectory.UserId == _currentUser.Id);

            return storageFile;
        }

        public async Task<long> GetUsedStorage(string path)
        {
            long usedSrorage = await _db.Files.Include(f => f.ParentDirectory).Where(f => f.ParentDirectory.UserId == _currentUser.Id).SumAsync(f => f.Size);

            return usedSrorage;
        }

        public async Task IncreaseDownloadsCounter(string filePath)
        {
            var file = await GetFile(filePath);
            file.DownloadsNumber++;

            _db.SaveChanges();
        }

        public async Task InitializeStorage()
        {
            try
            {
                await _db.Database.EnsureCreatedAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException("Server doesn't exists");
            }
        }

        public async Task<bool> IsEnoughStorageSpace(long fileSize)
        {
            long usedSpace = await GetUsedStorage(DirectoryPaths.InitialDirectoryPath);
            long maxStorage = Convert.ToInt64(ConfigurationManager.AppSettings["MaxStorage"]);

            return (usedSpace + fileSize) < maxStorage;
        }

        public bool IsFileSizeLessThanMaxSize(long fileSize)
        {
            var maxFileSize = Convert.ToInt64(ConfigurationManager.AppSettings["MaxFileSize"]);

            return fileSize < maxFileSize;
        }

        public async Task MoveDirectory(string oldPath, string newPath)
        {
            var movableDirectory = await GetDirectory(oldPath);

            if (_db.Directories.Any(d => d.Path == newPath))
            {
                throw new ArgumentException($"The directory with the same name is already exists at the path '{newPath}'");
            }

            string newParentDirectoryPath = PathHelper.GetParentDirectoryPath(newPath);
            var newParentDirectory = await GetDirectory(newParentDirectoryPath);

            movableDirectory.ParentDirectory.ModificationDate = DateTime.Now;
            newParentDirectory.ModificationDate = DateTime.Now;

            movableDirectory.ParentDirectory = newParentDirectory;
            movableDirectory.Path = newPath;
            movableDirectory.Name = Path.GetFileName(newPath);
            movableDirectory.ModificationDate = DateTime.Now;
            ChangeInnerDirectoriesPath(oldPath, movableDirectory.Path);

            await _db.SaveChangesAsync();
        }

        public async Task MoveFile(string oldFilePath, string newFilePath)
        {
            var movableFile = await GetFile(oldFilePath);

            if (movableFile == null)
            {
                throw new ArgumentException($"The file '{oldFilePath}' is not exists");
            }

            string newFileName = Path.GetFileName(newFilePath);
            string newParentDerictoryPath = PathHelper.GetParentDirectoryPath(newFilePath);

            if (await _db.Files.AnyAsync(f => f.Path == newFilePath))
            {
                throw new ArgumentException($"The file '{newFileName}' is already exists in the directory {newParentDerictoryPath}");
            }

            var newParentDirectory = await GetDirectory(newParentDerictoryPath);

            movableFile.ParentDirectory.ModificationDate = DateTime.Now;
            movableFile.Name = newFileName;
            movableFile.Extension = Path.GetExtension(newFileName);
            movableFile.Path = $"{newParentDirectory.Path}/{newFileName}";

            movableFile.ParentDirectory = newParentDirectory;
            newParentDirectory.ModificationDate = DateTime.Now;

            _db.SaveChanges();
        }

        public async Task RemoveDirectory(string path)
        {
            var directory = await GetDirectory(path);

            if (directory.Path == DirectoryPaths.InitialDirectoryPath)
            {
                throw new ArgumentException("You can't delete the initial root directory");
            }

            //deleting all inner directories
            _db.Directories.RemoveRange(_db.Directories.Where(d => d.Path.StartsWith($"{directory.Path}/") && d.UserId == _currentUser.Id));
            _db.Directories.Remove(directory);

            await _db.SaveChangesAsync();
        }

        public async Task RemoveFile(string filePath)
        {
            var file = await GetFile(filePath);

            if (file == null)
            {
                throw new ArgumentException($"The file {filePath} not exists");
            }

            file.ParentDirectory.Files.Remove(file);

            _db.SaveChanges();
        }

        private void FillDirectoryTree(StorageDirectory directory)
        {
            directory.ParentDirectory = _db.Directories.FirstOrDefault(d => d.Id == directory.ParentId);
            directory.Files = _db.Files.Where(f => f.DirectoryId == directory.Id).ToList();
            directory.Directories = _db.Directories.Where(d => d.ParentId == directory.Id).ToList();

            foreach (var innerDir in directory.Directories)
            {
                FillDirectoryTree(innerDir);
            }
        }

        private void ChangeInnerDirectoriesPath(string oldPath, string newPath)
        {
            var innerDirectories = _db.Directories.Where(d => d.Path.StartsWith(oldPath) && d.UserId == _currentUser.Id);
            foreach (var innerDirectory in innerDirectories)
            {
                innerDirectory.Path = innerDirectory.Path.Replace(oldPath, newPath);

                foreach (var file in innerDirectory.Files)
                {
                    file.Path = file.Path.Replace(oldPath, newPath);
                }
            }
        }
    }
}
