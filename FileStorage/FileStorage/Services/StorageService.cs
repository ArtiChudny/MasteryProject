using FileStorage.Models;
using System;
using System.Globalization;

namespace FileStorage.Services
{
    public class StorageService
    {
        public StorageInfo GetStorageInfo()
        {
            return new StorageInfo
            {
                UsedStorage = 512,
                CreationDate = DateTime.Now
            };
        }
    }
}
