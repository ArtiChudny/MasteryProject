using FileStorage.Enums;
using System.Collections.Generic;

namespace FileStorage.Models
{
    public class Options
    {
        public IList<string> Parameters { get; set; }
        public IDictionary<StorageFlags, string> Flags { get; set; }

        public Options()
        {
            Parameters = new List<string>();
            Flags = new Dictionary<StorageFlags, string>();
        }
    }
}
