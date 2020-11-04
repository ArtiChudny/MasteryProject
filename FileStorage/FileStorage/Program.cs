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
            }
            else
            {
                Environment.Exit(-1);
            }
        }
    }
}
