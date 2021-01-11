using System;

namespace FileStorage.DAL.Models
{
    public class CommandExecutionInfo
    {
        public int Id { get; set; }
        public string CommandName { get; set; }
        public double ExecutionTime { get; set; }
        public DateTime ExecutionDate { get; set; }
    }
}
