using FileStorage.ConsoleUI.Enums;
using System.Collections.Generic;

namespace FileStorage.ConsoleUI.Models
{
    public class Options
    {
        public IList<string> Parameters { get; set; }
        public Dictionary<StorageFlags, string> Flags { get; set; }

        public Options()
        {
            Parameters = new List<string>();
            Flags = new Dictionary<StorageFlags, string>();
        }
    }
}
