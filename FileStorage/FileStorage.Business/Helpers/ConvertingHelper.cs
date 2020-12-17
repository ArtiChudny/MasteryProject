using FileStorage.DAL.Models;
using System.Collections.Generic;

namespace FileStorage.BLL.Helpers
{
    public class ConvertingHelper
    {
        public static SerializableStorageInfo GetSerializableStorageInfo(StorageInfo storageInfo)
        {
            SerializableStorageInfo serializableStorageInfo = new SerializableStorageInfo
            {
                CreationDate = storageInfo.CreationDate,
                UsedStorage = storageInfo.UsedStorage
            };

            GetInnerFiles(serializableStorageInfo.Directories, storageInfo.Directories);

            return serializableStorageInfo;
        }

        private static void GetInnerFiles(List<SerializableStorageDirectoryKeyValuePair> Directories, Dictionary<string, StorageDirectory> directories)
        {
            foreach (var dir in directories)
            {
                var serializablePair = new SerializableStorageDirectoryKeyValuePair
                {
                    Key = dir.Key,
                    Value = ConvertToSerializableStorageDirectory(dir.Value)
                };

                Directories.Add(serializablePair);

                if (dir.Value.Directories.Count != 0)
                {
                    GetInnerFiles(Directories.Find(x => x == serializablePair).Value.Directories, dir.Value.Directories);
                }
            }
        }

        private static SerializableStorageDirectory ConvertToSerializableStorageDirectory(StorageDirectory directory)
        {
            var serializableDirectory = new SerializableStorageDirectory()
            {
                CreationDate = directory.CreationDate,
                ModificationDate = directory.ModificationDate,
                Name = directory.Name,
                ParentId = directory.ParentId,
            };

            foreach (var file in directory.Files)
            {
                var serializablePair = new SerializableStorageFileKeyValuePair()
                {
                    Key = file.Key,
                    Value = file.Value
                };

                serializableDirectory.Files.Add(serializablePair);
            }

            return serializableDirectory;
        }
    }
}
