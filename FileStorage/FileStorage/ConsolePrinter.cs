using System;
using FileStorage.ViewModels;

namespace FileStorage
{
    public class ConsolePrinter
    {
        public void PrintUserInformation(UserInfoViewModel userInfo)
        {
            Console.WriteLine("\nlogin: {0}", userInfo.Login);
            Console.WriteLine("creation date: {0}", userInfo.CreationDate);
            Console.WriteLine("storage used: {0}MB\n", userInfo.UsedStorage);
        }

        public void PrintAuthenticationSuccessful()
        {
            Console.WriteLine("You logged in.\n");
        }

        public void PrintСommandWaitingIcon()
        {
            Console.Write(">");
        }

        public void PrintErrorMessage(string errorMessage)
        {
            Console.WriteLine("\n{0}\n", errorMessage);
        }

        public void PrintExitMessage()
        {
            Console.WriteLine("You have exit the application");
        }
    }
}
