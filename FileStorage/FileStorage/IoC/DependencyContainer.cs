using FileStorage.BLL;
using FileStorage.BLL.Interfaces;
using FileStorage.ConsoleUI.ConsoleUtils;
using FileStorage.ConsoleUI.ConsoleUtils.Interfaces;
using FileStorage.DAL.Repositories;
using FileStorage.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FileStorage.ConsoleUI.IoC
{
    public class DependencyContainer
    {
        public  IServiceProvider GetContainer()
        {
            ServiceCollection container = new ServiceCollection();

            container.AddTransient<IFileRepository, FileRepository>();
            container.AddTransient<IStorageRepository, StorageRepository>();
            container.AddTransient<IUserRepository, UserRepository>();
            container.AddTransient<IStorageFileService, StorageFileService>();
            container.AddTransient<IUserService, UserService>();
            container.AddTransient<IAuthService, AuthService>();
            container.AddTransient<IConsolePrinter, ConsolePrinter>();
            container.AddTransient<Controller>();

            return container.BuildServiceProvider();
        }
    }
}
