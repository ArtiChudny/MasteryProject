using FileStorage.Models;
using System;
using System.IO;
using System.Runtime.Serialization;

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
            if (!storageService.IsFileExists(fileName))
            {
                throw new ApplicationException($"File '{fileName}' is not exists");
            }

            return storageService.GetFileInfo(fileName);
        }

        public void RemoveStorageFile(string fileName)
        {
            if (!storageService.IsFileExists(fileName))
            {
                throw new ApplicationException($"File '{fileName}' is not exists");
            }

            string fileGuid = storageService.GetFileGuid(fileName);
            storageService.RemoveFile(fileName);
            fileService.RemoveFile(fileGuid);
        }

        public void MoveStorageFile(string oldFileName, string newFileName)
        {
            storageService.MoveFile(oldFileName, newFileName);
        }

        public StorageFile UploadStorageFile(string filePath)
        {
            StorageFile storageFile = fileService.GetStorageFile(filePath);
            string fileName = Path.GetFileName(filePath);

            if (storageService.IsFileExists(fileName))
            {
                throw new ApplicationException("A file with the same name already exists in the storage");
            }

            if (!storageService.IsFileSizeLessThanMaxSize(storageFile.Size))
            {
                throw new ApplicationException("The file exceeds the maximum size");
            }

            if (!storageService.IsEnoughStorageSpace(storageFile.Size))
            {
                throw new ApplicationException("Not enough space in the storage to upload the file");
            }

            fileService.UploadFileIntoStorage(filePath, storageFile.Guid);
            storageService.AddFileToStorage(fileName, storageFile);

            return storageFile;
        }

        public void DownloadStorageFile(string fileName, string destinationPath)
        {
            if (!storageService.IsFileExists(fileName))
            {
                throw new ApplicationException($"File {fileName} is not exists");
            }

            string fileGuid = storageService.GetFileGuid(fileName);

            if (!fileService.IsMd5HashMatch(fileGuid, storageService.GetStorageFileMD5Hash(fileName)))
            {
                throw new ApplicationException("The file has been damaged or changed");
            }

            fileService.DownloadFileFromStorage(fileName, fileGuid, destinationPath);
            storageService.IncreaseDownloadsCounter(fileName);
        }
    }
}
