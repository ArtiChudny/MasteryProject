using System.Collections.Generic;

namespace FileStorage.Models
{
    public class StorageCommand
    {
        public string CommandName { get; set; }
        public List<string> Options { get; set; }

        public StorageCommand()
        {
            CommandName = string.Empty;
            Options = new List<string>();
        }
    }
    
}
