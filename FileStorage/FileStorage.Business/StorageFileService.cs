using FileStorage.BLL.Helpers;
using FileStorage.BLL.Interfaces;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using System;
using System.IO;

namespace FileStorage.BLL
{
    public class StorageFileService: IStorageFileService
    {
        private IStorageRepository storageProvider;
        private IFileRepository fileProvider;

        public StorageFileService(IStorageRepository storageProvider, IFileRepository fileProvider)
        {
            this.storageProvider = storageProvider;
            this.fileProvider = fileProvider;
        }

        public StorageInfo GetStorageInfo()
        {
            return storageProvider.GetStorageInfo();
        }

        public StorageFile GetFileInfo(string fileName)
        {
            StorageFile storageFile = storageProvider.GetFileInfo(fileName);

            if (storageFile == null)
            {
                throw new ApplicationException($"File '{fileName}' is not exists");
            }

            return storageFile;
        }

        public void RemoveStorageFile(string fileName)
        {
            StorageFile storageFile = storageProvider.GetFileInfo(fileName);

            if (storageFile == null)
            {
                throw new ApplicationException($"File '{fileName}' is not exists");
            }

            string storageFileName = storageFile.Id.ToString();
            storageProvider.RemoveFile(fileName);
            fileProvider.RemoveFile(storageFileName);
        }

        public void MoveStorageFile(string oldFileName, string newFileName)
        {
            storageProvider.MoveFile(oldFileName, newFileName);
        }

        public StorageFile UploadStorageFile(string filePath)
        {
            FileInfoModel fileInfo = fileProvider.GetFileInfo(filePath);
            string fileName = Path.GetFileName(filePath);

            if (storageProvider.GetFileInfo(fileName) != null)
            {
                throw new ApplicationException("A file with the same name already exists in the storage");
            }

            if (!storageProvider.IsFileSizeLessThanMaxSize(fileInfo.Size))
            {
                throw new ApplicationException("The file exceeds the maximum size");
            }

            if (!storageProvider.IsEnoughStorageSpace(fileInfo.Size))
            {
                throw new ApplicationException("Not enough space in the storage to upload the file");
            }

            StorageFile storageFile = storageProvider.CreateFile(fileName, fileInfo.Size, fileInfo.Hash, fileInfo.CreationDate);
            string storageFileName = storageFile.Id.ToString();
            fileProvider.UploadFileIntoStorage(filePath, storageFileName);

            return storageFile;
        }

        public void DownloadStorageFile(string fileName, string destinationPath)
        {
            StorageFile storageFile = storageProvider.GetFileInfo(fileName);

            if (storageFile == null)
            {
                throw new ApplicationException($"File '{fileName}' is not exists");
            }

            if (!fileProvider.IsHashMatch(storageFile.Id.ToString(), storageFile.Hash))
            {
                throw new ApplicationException("The file has been damaged or changed");
            }

            string storageFileName = storageFile.Id.ToString();
            fileProvider.DownloadFileFromStorage(fileName, storageFileName, destinationPath);
            storageProvider.IncreaseDownloadsCounter(fileName);
        }

        public void ExportStorageInfoFile(string destinationPath, string format)
        {
            StorageInfo storageInfo = storageProvider.GetStorageInfo();
            SerializableStorageInfo serializableStorageInfo = ConvertingHelper.GetSerializableStorageInfo(storageInfo);
            fileProvider.ExportFile(serializableStorageInfo, destinationPath, format);
        }

        public void InitializeStorage()
        {
            storageProvider.InitializeStorage();
            fileProvider.InitializeFileStorage();
        }

        public void CreateStorageDirectory(string destinationPath, string directoryName)
        {
            storageProvider.CreateDirectory(destinationPath, directoryName);
        }
    }
}
