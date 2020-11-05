using System;
using System.Collections.Generic;
using FileStorage.Enums;

namespace FileStorage
{
    public static class ConsoleFlagParser
    {
        private const string LoginFlag = "--l";
        private const string PasswordFlag = "--p";

        public static Dictionary<StorageFlags, string> Parse(string[] args)
        {
            Dictionary<StorageFlags, string> flagsValues = new Dictionary<StorageFlags, string>();
            for (int argIndex = 0; argIndex < args.Length; argIndex++)
            {
                StorageFlags flag = GetFlag(args[argIndex]);
                string value = string.Empty;
                if ((argIndex + 1) < args.Length)
                {
                    value = args[argIndex + 1];
                }    
                if (flagsValues.ContainsKey(flag))
                {
                    throw new ApplicationException($"Flag {flag} repeats.");
                }
                else
                {
                    flagsValues.Add(flag, value);
                    argIndex++;
                }             
            }
            return flagsValues;
        }

        private static StorageFlags GetFlag(string flagName)
        {
            switch (flagName)
            {
                case LoginFlag:
                    return StorageFlags.Login;
                case PasswordFlag:
                    return StorageFlags.Password;
                default:
                    throw new ApplicationException($"Wrong flag: {flagName}.");
            }
        }
    }
}
