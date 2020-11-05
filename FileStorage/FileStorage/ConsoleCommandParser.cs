using FileStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static void GetCredentialsFromInitialParameters(string[] args, ref string login, ref string password)
        {
            if (args[0] == "--l" && args[2] == "--p")
            {
                login = args[1];
                password = args[3];
            }
            else
            {
                login = args[3];
                password = args[1];
            }
        }
    }
}
