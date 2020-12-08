using FileStorage.DAL.Models;
using System;
using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories.Interfaces
{
    public interface IStorageRepository
    {
        void InitializeStorage();
        Task<StorageInfo> GetStorageInfo();
        Task<StorageFile> CreateFile(string directoryPath, string fileName, long fileSize, byte[] hash, DateTime creationDate);
        Task<StorageFile> GetFileInfo(string filePath);
        byte[] GetFileHash(string fileName);
        void IncreaseDownloadsCounter(string filePath);
        bool IsEnoughStorageSpace(long fileSize);
        bool IsFileSizeLessThanMaxSize(long fileSize);
        Task MoveFile(string oldFilePath, string newFilePath);
        Task RemoveFile(string filePath);
        Task CreateDirectory(string path, string directoryName);
        Task MoveDirectory(string oldPath, string newPath);
        Task<StorageDirectory> GetDirectory(string path);
        Task RemoveDirectory(string path);
    }
}