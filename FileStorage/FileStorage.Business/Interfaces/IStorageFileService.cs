using FileStorage.DAL.Models;

namespace FileStorage.BLL.Interfaces
{
    public interface IStorageFileService
    {
        void InitializeStorage();
        StorageInfo GetStorageInfo();
        StorageFile GetFileInfo(string fileName);
        void RemoveStorageFile(string fileName);
        void MoveStorageFile(string oldFileName, string newFileName);
        StorageFile UploadStorageFile(string filePath);
        void DownloadStorageFile(string fileName, string destinationPath);
        void ExportFile(string destinationPath, string format);
    }
}