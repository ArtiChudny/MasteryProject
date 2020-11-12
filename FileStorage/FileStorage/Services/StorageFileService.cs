using FileStorage.Models;
using System;

namespace FileStorage.Services
{
    public class StorageFileService
    {
        private StorageService storageService;
        private FileService fileService;

        public StorageFileService(StorageService storageService, FileService fileService)
        {
            this.storageService = storageService;
            this.fileService = fileService;
        }

        public StorageInfo GetStorageInfo()
        {
            return storageService.GetStorageInfo();
        }

        public StorageFile GetFileInfo(string fileName)
        {
            return storageService.GetFileInfo(fileName);
        }

        public void RemoveStorageFile(string fileName)
        {
            fileService.RemoveFile(fileName);
            storageService.RemoveFile(fileName);
        }

        public void MoveStorageFile(string oldFileName, string newFileName)
        {
            fileService.MoveFile(oldFileName, newFileName);
            storageService.MoveFile(oldFileName, newFileName);
        }

        public StorageFile UploadStorageFile(string filePath)
        {
            StorageFile storageFile = fileService.GetStorageFile(filePath);

            if (!storageService.IsFileSizeLessThanMaxSize(storageFile.Size))
            {
                throw new ApplicationException("The file exceeds the maximum size");
            }

            if (!storageService.IsEnoughStorageSpace(storageFile.Size))
            {
                throw new ApplicationException("Not enough space in storage to upload the file");
            }

            fileService.UploadFileIntoStorage(filePath);
            storageService.AddFileToStorage(storageFile);

            return storageFile;
        }

        public void DownloadStorageFile(string fileName, string destinationPath)
        {
            if (!fileService.IsMd5HashMatch(fileName, storageService.GetStorageFileMD5Hash(fileName)))
            {
                throw new ApplicationException("File has been damaged or changed");
            }
            fileService.DownloadFileFromStorage(fileName, destinationPath);
            storageService.IncreaseDownloadsCounter(fileName);
        }
    }
}
