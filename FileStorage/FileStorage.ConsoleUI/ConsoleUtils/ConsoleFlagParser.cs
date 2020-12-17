using System;
using System.Collections.Generic;
using FileStorage.BLL.Enums;

namespace FileStorage.ConsoleUI.ConsoleUtils
{
    public class ConsoleFlagParser
    {
        private const string FlagIndicator = "--";
        private const string LoginFlag = "--l";
        private const string PasswordFlag = "--p";
        private const string FormatFlag = "--format";
        private const string InfoFlag = "--info";

        public Dictionary<StorageFlags, string> Parse(string[] args)
        {
            var flags = new Dictionary<StorageFlags, string>();

            for (int argIndex = 0; argIndex < args.Length; argIndex++)
            {
                StorageFlags flag = GetFlag(args[argIndex]);
                if (flags.ContainsKey(flag))
                {
                    throw new ApplicationException($"Flag {flag} repeats.");
                }

                //checking that the next argument is a flag value, because not all flags need it
                var flagValue = string.Empty;
                if (((argIndex + 1) < args.Length) && (IsFlagValue(args[argIndex + 1])))
                {
                    flagValue = args[argIndex + 1];
                    argIndex++;
                }

                flags.Add(flag, flagValue);
            }

            return flags;
        }

        private StorageFlags GetFlag(string flagName)
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

        private bool IsFlagValue(string arg)
        {
            return !arg.StartsWith(FlagIndicator);
        }
    }
}
