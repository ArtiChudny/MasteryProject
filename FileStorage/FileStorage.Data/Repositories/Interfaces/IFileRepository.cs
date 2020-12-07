using FileStorage.DAL.Models;
using System.Threading.Tasks;

namespace FileStorage.DAL.Repositories.Interfaces
{
    public interface IFileRepository
    {
        Task UploadFileIntoStorage(string filePath, string guid);
        Task DownloadFileFromStorage(string fileName, string storageFileName, string destinationPath);
        Task<FileInfoModel> GetFileInfo(string filePath);
        Task MoveFile(string oldFileName, string newFileName);
        Task RemoveFile(string fileName);
        byte[] GetFileHash(string filePath);
        bool IsHashMatch(string fileName, byte[] storageFileHash);
        void InitializeFileStorage();
        Task ExportFile(SerializableStorageInfo storageInfo, string destinationPath, string format);
    }
}
