using FileStorage.DAL.Models;

namespace FileStorage.DAL.Repositories.Interfaces
{
    public interface IFileRepository
    {
        void UploadFileIntoStorage(string filePath, string guid);
        void DownloadFileFromStorage(string fileName, string storageFileName, string destinationPath);
        FileInfoModel GetFileInfo(string filePath);
        void MoveFile(string oldFileName, string newFileName);
        void RemoveFile(string fileName);
        byte[] GetFileHash(string filePath);
        bool IsHashMatch(string fileName, byte[] storageFileHash);
        void InitializeFileStorage();
        void ExportFile(SerializableStorageInfo storageInfo, string destinationPath, string format);
    }
}
