using System.Collections.Generic;

namespace FileStorage.Models
{
    public class StorageCommand
    {
        public string CommandName { get; set; }
        public List<string> Parameters { get; set; }

        public StorageCommand()
        {
            CommandName = string.Empty;
            Parameters = new List<string>();
        }
    }
    
}
