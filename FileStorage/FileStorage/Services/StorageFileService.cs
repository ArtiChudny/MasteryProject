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
            if (storageService.GetFileInfo(fileName) == null)
            {
                throw new ApplicationException($"File '{fileName}' is not exists");
            }

            return storageService.GetFileInfo(fileName);
        }

        public void RemoveStorageFile(string fileName)
        {
            if (storageService.GetFileInfo(fileName) == null)
            {
                throw new ApplicationException($"File '{fileName}' is not exists");
            }

            Guid fileGuid = storageService.GetFileGuid(fileName);
            storageService.RemoveFile(fileName);
            fileService.RemoveFile(fileGuid.ToString());
        }

        public void MoveStorageFile(string oldFileName, string newFileName)
        {
            storageService.MoveFile(oldFileName, newFileName);
        }

        public StorageFile UploadStorageFile(string filePath)
        {
            FileInfoModel fileInfo = fileService.GetStorageFile(filePath);
            string fileName = Path.GetFileName(filePath);

            if (storageService.GetFileInfo(fileName) != null)
            {
                throw new ApplicationException("A file with the same name already exists in the storage");
            }

            if (!storageService.IsFileSizeLessThanMaxSize(fileInfo.Size))
            {
                throw new ApplicationException("The file exceeds the maximum size");
            }

            if (!storageService.IsEnoughStorageSpace(fileInfo.Size))
            {
                throw new ApplicationException("Not enough space in the storage to upload the file");
            }

            StorageFile storageFile = storageService.CreateNewStorageFile(fileName, fileInfo.Size, fileInfo.Hash, fileInfo.CreationDate);
            fileService.UploadFileIntoStorage(filePath, storageFile.Id.ToString()) ;

            return storageFile;
        }

        public void DownloadStorageFile(string fileName, string destinationPath)
        {
            if (storageService.GetFileInfo(fileName) == null)
            {
                throw new ApplicationException($"File {fileName} is not exists");
            }

            StorageFile storageFile = storageService.GetFileInfo(fileName);

            if (!fileService.IsHashMatch(storageFile.Id.ToString(), storageFile.Hash))
            {
                throw new ApplicationException("The file has been damaged or changed");
            }

            fileService.DownloadFileFromStorage(fileName, storageFile.Id.ToString(), destinationPath);
            storageService.IncreaseDownloadsCounter(fileName);
        }
    }
}
