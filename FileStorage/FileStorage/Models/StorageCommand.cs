using System.Collections.Generic;

namespace FileStorage.Models
{
    class Command
    {
        public string CommandName { get; set; }
        public List<string> Options { get; set; }
    }
}
