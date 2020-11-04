using System;

namespace FileStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            if (StorageCommand.LoginToApp(args))
            {
                Console.WriteLine("You logged in.");
                while (true)
                {
                    Console.Write(">");
                    string currentCommand = Console.ReadLine().ToLower();
                    StorageCommand.ExecuteConsoleCommand(currentCommand);
                }
            }
            else
            {
                ConsolePrinter.PrintBadInitialParameters();
                StorageCommand.ExitApplication();
            }
        }
    }
}
