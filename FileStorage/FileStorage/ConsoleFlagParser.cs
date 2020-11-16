using System;
using System.Collections.Generic;
using FileStorage.Enums;

namespace FileStorage
{
    public class ConsoleFlagParser
    {
        private const string LoginFlag = "--l";
        private const string PasswordFlag = "--p";
        private const string FormatFlag = "--format";
        private const string InfoFlag = "--info";


        public Dictionary<StorageFlags, string> Parse(string[] args)
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

        public StorageFlags GetFlag(string flagName)
        {
            switch (flagName)
            {
                case LoginFlag:
                    return StorageFlags.Login;
                case PasswordFlag:
                    return StorageFlags.Password;
                case InfoFlag:
                    return StorageFlags.Info;
                case FormatFlag:
                    return StorageFlags.Format;
                default:
                    throw new ApplicationException($"Wrong flag: {flagName}.");
            }
        }
    }
}
