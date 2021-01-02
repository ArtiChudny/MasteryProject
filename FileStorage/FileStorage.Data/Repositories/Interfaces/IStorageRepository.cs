using FileStorage.DAL.Models;
using System;
using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories.Interfaces
{
    public interface IStorageRepository
    {
        Task InitializeStorage();
        Task<long> GetUsedStorage(string path);
        Task<StorageFile> CreateFile(string directoryPath, string fileName, long fileSize, byte[] hash, DateTime creationDate);
        Task<StorageFile> GetFile(string filePath);
        Task IncreaseDownloadsCounter(string filePath);
        Task<bool> IsEnoughStorageSpace(long fileSize);
        bool IsFileSizeLessThanMaxSize(long fileSize);
        Task MoveFile(string oldFilePath, string newFilePath);
        Task RemoveFile(string filePath);
        Task CreateDirectory(string path, string directoryName);
        Task MoveDirectory(string oldPath, string newPath);
        Task<StorageDirectory> GetDirectory(string path);
        Task<StorageDirectory> GetFullDirectoryTree(string path);
        Task RemoveDirectory(string path);
    }
}