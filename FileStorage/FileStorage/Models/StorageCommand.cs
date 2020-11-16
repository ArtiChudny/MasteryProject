using FileStorage.Enums;

namespace FileStorage.Models
{
    public class StorageCommand
    {
        public StorageCommands CommandType { get; set; }
        public Options Options { get; set; }
    }
}
