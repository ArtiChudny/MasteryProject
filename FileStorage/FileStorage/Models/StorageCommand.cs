using FileStorage.ConsoleUI.Enums;

namespace FileStorage.ConsoleUI.Models
{
    public class StorageCommand
    {
        public StorageCommands CommandType { get; set; }
        public Options Options { get; set; }
    }
}
