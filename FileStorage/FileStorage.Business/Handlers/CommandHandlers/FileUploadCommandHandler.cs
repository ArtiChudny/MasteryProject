using FileStorage.BLL.Commands;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.CommandHandlers
{
    public class FileUploadCommandHandler : IRequestHandler<FileUploadCommand, StorageFile>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IFileRepository _fileRepository;

        public FileUploadCommandHandler(IStorageRepository storageRepository, IFileRepository fileRepository)
        {
            _storageRepository = storageRepository;
            _fileRepository = fileRepository;
        }

        public async Task<StorageFile> Handle(FileUploadCommand request, CancellationToken cancellationToken)
        {
            FileInfoModel fileInfo = await _fileRepository.GetFileInfo(request.FilePath);

            string fileName = Path.GetFileName(request.FilePath);
            string storageFilePath = $"{request.DestinationDirectoryPath}/{fileName}";

            if (await _storageRepository.GetFileInfo(storageFilePath) != null)
            {
                throw new ArgumentException("A file with the same name already exists in the storage");
            }

            if (!_storageRepository.IsFileSizeLessThanMaxSize(fileInfo.Size))
            {
                throw new ArgumentException("The file exceeds the maximum size");
            }

            if (!_storageRepository.IsEnoughStorageSpace(fileInfo.Size))
            {
                throw new ArgumentException("Not enough space in the storage to upload the file");
            }

            StorageFile storageFile = await _storageRepository.CreateFile(request.DestinationDirectoryPath, fileName, fileInfo.Size, fileInfo.Hash, fileInfo.CreationDate);
            string guidFileName = storageFile.Id.ToString();
            await _fileRepository.UploadFileIntoStorage(request.FilePath, guidFileName);

            return storageFile;
        }
    }
}
