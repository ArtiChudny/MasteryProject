using System.Collections.Generic;
using FileStorage.Enums;

namespace FileStorage.Models
{
    public class StorageCommand
    {
        public StorageCommands CommandType { get; set; }
        public IList<string> Parameters { get; set; }

        public StorageCommand()
        {
            Parameters = new List<string>();
        }
    }

}
