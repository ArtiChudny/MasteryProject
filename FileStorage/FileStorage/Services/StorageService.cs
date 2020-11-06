using FileStorage.Models;
using System;

namespace FileStorage.Services
{
    public static class StorageService
    {
        public static StorageInfo GetStorageInfo()
        {
            return new StorageInfo
            {
                UsedStorage = 512,
                CreationDate = DateTime.Parse("2020-11-02")
            };
        }
    }
}
