using System;
using FileStorage.Models;

namespace FileStorage
{
    static class ConsolePrinter
    {
        public static void PrintUserInformation(User user)
        {
            Console.WriteLine("\nlogin: {0}", user.Login);
            Console.WriteLine("creation date: {0}", "2020-11-03");
            Console.WriteLine("storage used: {0}\n", "512MB");
        }
        public static void PrintAuthenticationSuccessful()
        {
            Console.WriteLine("You logged in.\n");
        }
        public static void PrintAuthenticationFailed()
        {
            Console.WriteLine("\nIncorrect login or password\n");
        }
        public static void PrintBadInitialParameters()
        {
            Console.WriteLine("Uncorrect initial parameters. Use --l for login and --p for password.");
        }

        public static void PrintBadParameter(string command, string badParameter)
        {
            Console.WriteLine("\nBad parameter {0} for command '{1}'\n", badParameter, command);
        }

        public static void PrintParameterNeeding(string commandName)
        {
            Console.WriteLine("\nCommand '{0}' needs at least one parameter\n", commandName);
        }

        public static void PrintWrongCommand(string commandName)
        {
            Console.WriteLine("\n{0} is not programm command.\n", commandName);
        }

        public static void PrintСommandWaitingIcon()
        {
            Console.Write(">");
        }

        public static void PrintErrorMessage(string errorMessage)
        {
            Console.WriteLine("\n{0}\n", errorMessage);
        }
    }
}
