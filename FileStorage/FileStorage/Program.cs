﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FileStorage.Enums;
using FileStorage.Models;
using FileStorage.Services;

namespace FileStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsolePrinter consolePrinter = new ConsolePrinter();
            Controller controller = new Controller(consolePrinter);
            AuthService authService = new AuthService();
            ConsoleCommandParser consoleCommandParser = new ConsoleCommandParser();

            CreateIfMissInitialDirectories();

            try
            {
                Dictionary<StorageFlags, string> flags = ConsoleFlagParser.Parse(args);
                Credentials credentials = GetCredentials(flags);
                if (!authService.IsAuthenticated(credentials))
                {
                    throw new ApplicationException("Incorrect login or password");
                }
                consolePrinter.PrintAuthenticationSuccessful();

                while (true)
                {
                    try
                    {
                        StorageCommand command = GetCommand(consolePrinter, consoleCommandParser);
                        if (command.CommandType == StorageCommands.Exit)
                        {
                            consolePrinter.PrintExitMessage();
                            break;
                        }
                        controller.ExecuteConsoleCommand(command);
                    }
                    catch (Exception ex)
                    {
                        consolePrinter.PrintErrorMessage(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                consolePrinter.PrintErrorMessage(ex.Message);
            }
        }

        private static Credentials GetCredentials(Dictionary<StorageFlags, string> flags)
        {
            if (IsContainLoginPassword(flags))
            {
                return new Credentials(flags[StorageFlags.Login], flags[StorageFlags.Password]);
            }
            else
            {
                throw new ApplicationException("You have to enter your login and password. Use --l for login and --p for password.");
            }
        }

        private static bool IsContainLoginPassword(Dictionary<StorageFlags, string> flags)
        {
            if (flags.ContainsKey(StorageFlags.Login) && flags.ContainsKey(StorageFlags.Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static StorageCommand GetCommand(ConsolePrinter consolePrinter, ConsoleCommandParser consoleCommandParser)
        {
            consolePrinter.PrintСommandWaitingIcon();
            string rowCommand = Console.ReadLine().ToLower().Trim();

            if (string.IsNullOrWhiteSpace(rowCommand))
            {
                throw new ApplicationException("You have not entered a command.");
            }

            return consoleCommandParser.Parse(rowCommand);
        }

        private static void CreateIfMissInitialDirectories()
        {
            string storagePath = ConfigurationManager.AppSettings["StoragePath"];
            string storageInfoPath = ConfigurationManager.AppSettings["StorageInfoPath"];

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
                Directory.CreateDirectory(Path.GetDirectoryName(storageInfoPath));
            }
            if (!File.Exists(storageInfoPath))
            {
                CreateNewStorageInfoFile();
            }
        }

        private static void CreateNewStorageInfoFile()
        {
            string storageInfoPath = ConfigurationManager.AppSettings["StorageInfoPath"];
           
            using (FileStream fs = new FileStream(storageInfoPath, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                StorageInfo storageInfo = new StorageInfo();
                formatter.Serialize(fs, storageInfo);
            }
        }
    }
}
