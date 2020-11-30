using FileStorage.BLL.Enums;

namespace FileStorage.BLL.Models
{
    public class StorageCommand
    {
        public StorageCommands CommandType { get; set; }
        public Options Options { get; set; }
    }
}
