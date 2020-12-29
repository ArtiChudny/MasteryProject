using FileStorage.DAL.Models;

namespace FileStorage.BLL.Helpers
{
    public static class ConvertingHelper
    {
        public static SerializableStorageDirectory GetSerializableInnerDirectory(StorageDirectory initialDirectory)
        {
            var serializableInitialDirectory = ConvertToSerializableDirectory(initialDirectory);

            GetInnerFiles(serializableInitialDirectory, initialDirectory);

            return serializableInitialDirectory;
        }

        private static void GetInnerFiles(SerializableStorageDirectory serializableDirectory, StorageDirectory directory)
        {
            foreach (var dir in directory.Directories)
            {
                var serDirectory = ConvertToSerializableDirectory(dir);

                serializableDirectory.Directories.Add(serDirectory);

                if (dir.Directories.Count != 0)
                {
                    GetInnerFiles(serDirectory, dir);
                }
            }
        }

        private static SerializableStorageDirectory ConvertToSerializableDirectory(StorageDirectory directory)
        {
            var serializableDirectory = new SerializableStorageDirectory()
            {
                CreationDate = directory.CreationDate,
                ModificationDate = directory.ModificationDate,
                Name = directory.Name,
                ParentId = directory.ParentId,
                Id = directory.Id,
                Path = directory.Path
            };

            foreach (var file in directory.Files)
            {
                var serializableFile = ConvertToSerializableFile(file);
                serializableDirectory.Files.Add(serializableFile);
            }

            return serializableDirectory;
        }

        private static SerializableStorageFile ConvertToSerializableFile(StorageFile file)
        {
            var serializableFile = new SerializableStorageFile()
            {
                CreationDate = file.CreationDate,
                DownloadsNumber = file.DownloadsNumber,
                Extension = file.Extension,
                GuidName = file.GuidName,
                Hash = file.Hash,
                Id = file.Id,
                Name = file.Name,
                Path = file.Path,
                Size = file.Size,
                StorageDirectoryId = file.DirectoryId
            };

            return serializableFile;
        }
    }
}