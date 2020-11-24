using FileStorage.DAL.Models;
using System;

namespace FileStorage.DAL.Repositories.Interfaces
{
    public interface IStorageRepository
    {
        void InitializeStorage();
        StorageInfo GetStorageInfo();
        StorageFile CreateFile(string fileName, long fileSize, byte[] hash, DateTime creationDate);
        StorageFile GetFileInfo(string fileName);
        byte[] GetFileHash(string fileName);
        void IncreaseDownloadsCounter(string fileName);
        bool IsEnoughStorageSpace(long fileSize);
        bool IsFileSizeLessThanMaxSize(long fileSize);
        void MoveFile(string oldFileName, string newFileName);
        void RemoveFile(string fileName);
        void CreateDirectory(string path, string directoryName);
    }
}