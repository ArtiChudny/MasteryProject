using FileStorage.DAL.Models;
using System;
using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories.Interfaces
{
    public interface IStorageRepository
    {
        void InitializeStorage();
        Task<StorageInfo> GetStorageInfo();
        Task<StorageFile> CreateFile(string fileName, long fileSize, byte[] hash, DateTime creationDate);
        Task<StorageFile> GetFileInfo(string fileName);
        byte[] GetFileHash(string fileName);
        void IncreaseDownloadsCounter(string fileName);
        bool IsEnoughStorageSpace(long fileSize);
        bool IsFileSizeLessThanMaxSize(long fileSize);
        Task MoveFile(string oldFileName, string newFileName);
        Task RemoveFile(string fileName);
        Task CreateDirectory(string path, string directoryName);
        Task MoveDirectory(string oldPath, string newPath);
        Task<StorageDirectory> GetDirectory(string path);
        Task RemoveDirectory(string path);
    }
}