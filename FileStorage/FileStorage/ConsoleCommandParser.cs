using System;
using System.Linq;
using FileStorage.Enums;
using FileStorage.Models;


namespace FileStorage
{
    static class ConsoleCommandParser
    {

        public static bool IsInitialParametersCorrect(string[] args)
        {
            if (args.Count() == 4 && (args[0] == "--l" || args[2] == "--l") && (args[0] == "--p" || args[2] == "--p"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
