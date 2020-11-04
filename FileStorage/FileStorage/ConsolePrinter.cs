using System;
using FileStorage.Models;

namespace FileStorage
{
    static class ConsolePrinter
    {
        public static void PrintStorageInformation()
        {
            Console.WriteLine("\nlogin: {0}", StorageInfo.login);
            Console.WriteLine("creation date: {0}", StorageInfo.creationDate);
            Console.WriteLine("max storage for use: {0}\n", StorageInfo.maxStorage);
        }

        public static void PrintBadParameter(string command, string badParameter)
        {
            Console.WriteLine("\nBad parameter {0} for command '{1}'\n", badParameter, command);
        }

        public static void PrintParameterNeeding(string commandName)
        {
            Console.WriteLine("\nCommand '{0}' needs at least one parameter\n", commandName);
        }

        public static void PrintAuthenticationFailed()
        {
            Console.WriteLine("\nIncorrect login or password\n");
        }

        public static void PrintWrongCommand(string command)
        {
            Console.WriteLine("\n{0} is not programm command.\n", command);
        }

        public static void PrintBadInitialParameters()
        {
            Console.WriteLine("Uncorrect initial parameters. Use --l for login and --p for password.");
        }
    }
}
