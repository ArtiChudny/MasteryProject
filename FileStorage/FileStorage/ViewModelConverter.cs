using System;
using FileStorage.Models;
using FileStorage.ViewModels;

namespace FileStorage
{
    public class ViewModelConverter
    {
        public FileInfoViewModel ConvertToFileInfoViewModel(User user, StorageFile storageFile)
        {
            return new FileInfoViewModel
            {
                FileName = storageFile.FileName,
                Extension = storageFile.Extension,
                CreationDate = GetDateString(storageFile.CreationDate),
                FileSize = GetSizeString(storageFile.Size),
                DownloadsNumber = storageFile.DownloadsNumber,
                Login = user.Login
            };
        }

        public FileUploadViewModel ConvertToFileUploadViewModel(string filePath, StorageFile storageFile)
        {
            return new FileUploadViewModel
            {
                FilePath = filePath,
                FileName = storageFile.FileName,
                FileSize = GetSizeString(storageFile.Size),
                Extension = storageFile.Extension
            };
        }

        public UserInfoViewModel ConvertToUserInfoViewModel(User user, StorageInfo storageInfo)
        {
            return new UserInfoViewModel
            {
                Login = user.Login,
                UsedStorage = GetSizeString(storageInfo.UsedStorage),
                CreationDate = GetDateString(storageInfo.CreationDate)
            };
        }

        private string GetDateString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        private string GetSizeString(long size)
        {
            if (size < 1024)
            {
                return size.ToString();
            }
            else if (size < 1048576)
            {
                return GetKbString(size);
            }
            else if (size < 1073741824)
            {
                return GetMbString(size);
            }
            else
            {
                return GetGbString(size);
            }
        }

        private string GetKbString(long size)
        {
            double MbSize = Math.Round((double)size / 1024, 2);

            return $"{MbSize}KB";
        }

        private string GetMbString(long size)
        {
            double MbSize = Math.Round((double)size / 1024 / 1024, 2);

            return $"{MbSize}MB";
        }

        private string GetGbString(long size)
        {
            double MbSize = Math.Round((double)size / 1024 / 1024 / 1024, 2);

            return $"{MbSize}GB";
        }
    }
}
