using FileStorage.Helpers;
using FileStorage.Models;
using System;
using System.IO;

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
            StorageFile storageFile = storageService.GetFileInfo(fileName);

            if (storageFile == null)
            {
                throw new ApplicationException($"File '{fileName}' is not exists");
            }

            return storageFile;
        }

        public void RemoveStorageFile(string fileName)
        {
            StorageFile storageFile = storageService.GetFileInfo(fileName);

            if (storageFile == null)
            {
                throw new ApplicationException($"File '{fileName}' is not exists");
            }

            string storageFileName = storageFile.Id.ToString();
            storageService.RemoveFile(fileName);
            fileService.RemoveFile(storageFileName);
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
            string storageFileName = storageFile.Id.ToString();
            fileService.UploadFileIntoStorage(filePath, storageFileName);

            return storageFile;
        }

        public void DownloadStorageFile(string fileName, string destinationPath)
        {
            StorageFile storageFile = storageService.GetFileInfo(fileName);

            if (storageFile == null)
            {
                throw new ApplicationException($"File '{fileName}' is not exists");
            }

            if (!fileService.IsHashMatch(storageFile.Id.ToString(), storageFile.Hash))
            {
                throw new ApplicationException("The file has been damaged or changed");
            }

            string storageFileName = storageFile.Id.ToString();
            fileService.DownloadFileFromStorage(fileName, storageFileName, destinationPath);
            storageService.IncreaseDownloadsCounter(fileName);
        }

        public void ExportFile(string destinationPath, string format)
        {
            StorageInfo storageInfo = storageService.GetStorageInfo();
            SerializableStorageInfo serializableStorageInfo = ConvertingHelper.GetSerializableStorageInfo(storageInfo);
            fileService.ExportFile(serializableStorageInfo, destinationPath, format);
        }
    }
}
